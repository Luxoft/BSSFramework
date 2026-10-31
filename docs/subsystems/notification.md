# Уведомления и подписки

Две связанные подсистемы:

- **Notification** (`src/Notification`) — отправка писем: очередь, обработка получателей и тестовых окружений, SMTP.
- **Subscriptions** (`src/Subscriptions`) — правила «при изменении доменного объекта X отправить письмо Y»: кому, с каким шаблоном и при каком условии.

Подписки описываются кодом (code-first) и реагируют на изменения через DAL-листенер `SubscriptionDALListener`. Готовое уведомление уходит в Notification.

## Подписка

Подписка — класс, наследующий `Subscription<TDomainObject, TRenderingObject>` (`Framework.Subscriptions.Metadata`). `TRenderingObject` — модель для шаблона письма. Часто это сам доменный объект.

Пример `SampleSystem.Subscriptions.Metadata/Country/Create/CountryCreateSubscription.cs`:

```csharp
public class CountryCreateSubscription : Subscription<Domain.Directories.Country, _Country_Create_MessageTemplate_cshtml>
{
    public override DomainObjectChangeType DomainObjectChangeType { get; } = DomainObjectChangeType.Create;

    public override bool InlineAttachments { get; } = false;

    public override async IAsyncEnumerable<NotificationMessageGenerationInfo<Domain.Directories.Country>> GetTo(
        IServiceProvider serviceProvider, DomainObjectVersions<Domain.Directories.Country> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }

    public override async IAsyncEnumerable<NotificationMessageGenerationInfo<Domain.Directories.Country>> GetCopyTo(
        IServiceProvider serviceProvider, DomainObjectVersions<Domain.Directories.Country> versions)
    {
        yield return new("tester@luxoft.com", versions);
    }
}
```

### Что можно переопределить

Контракт `ISubscription<TDomainObject, TRenderingObject>`:

| Член | Назначение |
|---|---|
| `DomainObjectChangeType` | Когда срабатывает подписка: `Create`, `Update`, `Delete` или `Any` (флаги) |
| `IsProcessed(...)` | Дополнительное условие: подписку надо обработать для этих версий объекта? |
| `ConvertToRenderingObject(...)` | Преобразование доменного объекта в модель для шаблона |
| `GetTo` / `GetCopyTo` / `GetReplyTo` | Получатели: основные, копия, Reply-To |
| `GetNotificationFilterGroups(...)` | Группы фильтров получателей по ролям безопасности |
| `GetMessage(...)` | Тема и тело письма (по умолчанию из шаблона) |
| `GetAttachments(...)` | Вложения |
| `Sender` | Адрес отправителя |
| `SendIndividualLetters` | `true` — отдельное письмо каждому получателю To / Copy вместо общего. ReplyTo добавляется в каждое письмо |
| `InlineAttachments` | Встраивать вложения из шаблона |
| `AllowEmptyListOfRecipients` | Разрешить отправку без получателей (например, для тестов) |
| `RecipientMergeType` | Как объединять получателей разных источников: `Union`, `Intersect`, `LeftExceptRight`, `RightExceptLeft` |
| `SecurityRule` | Правило безопасности для выбора получателей по ролям |

Версии объекта приходят в `DomainObjectVersions<T>` (предыдущая и текущая): сравнивайте их, чтобы решить, нужно ли письмо.

Получателей можно указать строкой с email через запятую или точку с запятой (`new("a@x.com; b@x.com", versions)`): адреса очищаются и дедуплицируются без учёта регистра. Для других типов получателей есть `NotificationMessageGenerationInfo<TRenderingObject, TRecipient>`.

### Шаблон письма

Рядом с подпиской лежит Razor-шаблон `MessageTemplate.cshtml` и сгенерированный по нему `MessageTemplate.generated.cs` (`RazorTemplate`). Класс шаблона и указывается вторым параметром `Subscription<…>`. Файл `.generated.cs` создаётся инструментом Razor-генерации, руками его править нельзя.

### Структура проекта метаданных

```
SampleSystem.Subscriptions.Metadata/
  Country/Create/
    CountryCreateSubscription.cs
    MessageTemplate.cshtml
    MessageTemplate.generated.cs
  DataModel/Create/...
  DomainChangedByRecipients/NotPersistentCustomModel/...
```

Принято группировать по доменному типу и типу изменения: `<Тип>/<Create|Update|…>/`.

## Подключение

В `AddGeneralDependencyInjection`:

```csharp
.AddSubscriptions(
    (Employee e) => e.Email,                                              // как получить email сотрудника
    [typeof(EmployeeUpdateSubscription).Assembly],                        // сборки с подписками
    new("SampleSystem@luxoft.com", "SampleSystem"))                       // адрес и имя отправителя по умолчанию
.AddSubscriptionManagers()                                                // см. ниже
.AddLegacyNotification(configuration, hostEnvironment)
```

Дополнительно регистрируется листенер `SubscriptionDALListener`:

```csharp
settings.AddListener<SubscriptionDALListener>()
```

`AddLegacyNotification` в SampleSystem вызывает `settings.AddNotification(configuration, isProduction)` и подменяет отправитель на `LocalDbNotificationMessageSender`, который складывает письма в локальную БД вместо реальной отправки. Для продакшена оставьте реализацию по умолчанию (SMTP).

## Отправка письма

Цепочка в `Framework.Notification.Runtime`:

1. `RootNotificationMessageSender` принимает `Notification`. Если получателей нет, пишет предупреждение и перенаправляет письмо на поддержку.
2. `IActualNotificationProcessor` → `ActualMailMessageProcessor`: клонирует сообщение и применяет цепочку `IMailMessageModifier` **в порядке регистрации в DI**. Каждый видит результат предыдущего, поэтому порядок важен (например, перенаправление на тестовый адрес должно идти раньше всего, что опирается на итоговых получателей).
3. Внутренний отправитель (по ключу `RootNotificationMessageSender.InnerKey`): SMTP (`SmtpNotificationMessageSender`) или свой.
4. `ISentNotificationLogger` фиксирует факт отправки.

Готовые модификаторы (`MailMessageModifier/`):

| Модификатор | Что делает |
|---|---|
| `RedirectToSupportMailMessageModifier` | Если нет ни одного получателя (To / CC / Bcc), добавляет `DefaultReceiverEmails`. Должен идти после модификаторов, меняющих список получателей |
| `RedirectToTestAddress` | Если заданы `TestEmails`, удаляет всех реальных получателей (To / CC / Bcc / ReplyTo) и отправляет только на тестовые адреса. Исходных получателей вставляет в начало тела письма (тело считается HTML) |
| `RewriteReceiversMailMessageModifier` | Заменяет адреса по правилам `RewriteReceiversSettings`. **Если задано хотя бы одно правило, получатели без подходящего правила отбрасываются.** Без правил письмо не меняется |
| `SubjectCleanerMailMessageModifier` | Заменяет переводы строк в теме на пробелы и обрезает тему до `MaxSubjectLength` |
| `GdprMailMessageModifier` | Вырезает из тела секции `<!--GDPR-->…<!--/GDPR-->` (регистр не важен, вложенные секции обрабатываются изнутри). Незакрытая секция остаётся в тексте |
| `HtmlMarkerMessageModifier` | Ставит `IsBodyHtml = true`, если тело начинается с `<html` |

Свой модификатор: реализуйте `IMailMessageModifier` и зарегистрируйте в DI (позиция в порядке регистрации определяет момент его вызова).

> Чтобы секция шаблона не попала в письмо, оберните её в `<!--GDPR-->…<!--/GDPR-->`, при условии что `GdprMailMessageModifier` зарегистрирован в вашем приложении.

## Конфигурация

Пример из `appsettings.json` SampleSystem:

```json
"SmtpSettings": {
  "SmtpEnabled": "false",
  "OutputFolder": "C:\\Logs\\SampleSystem\\emails",
  "Server": "puppy",
  "Port": 25,
  "DefaultReceiverEmails": [ "test3@luxoft.fake" ],
  "TestEmails": [ "IadTest@luxoft.fake" ]
},
"RewriteReceiversSettings": {
  "RewriteRules": [
    { "From": "test1@luxoft.fake", "To": [ "test2@luxoft.fake" ] }
  ]
}
```

`SmtpSettings`:

| Параметр | Смысл |
|---|---|
| `SmtpEnabled` | `false` — не отправлять на SMTP-сервер (для разработки) |
| `OutputFolder` | Каталог, куда складываются письма |
| `Server`, `Port` | SMTP-сервер, порт по умолчанию 25 |
| `DefaultReceiverEmails` | Адреса для писем без получателей (когда пустой список допустим) |
| `TestEmails` | Тестовые адреса для окружений, отличных от production |
| `MaxSubjectLength` | Максимальная длина темы, по умолчанию без ограничения |

`RewriteReceiversSettings.RewriteRules`: список правил `From` → `To[]`.

## Менеджеры подписок

`AddSubscriptionManager<T>()` регистрирует «менеджеры подписок» приложения (в SampleSystem: `ExampleSampleSystemEventsSubscriptionManager`, `ExampleSampleSystemAribaEventsSubscriptionManager`). Они относятся к старому API (блок `// Legacy` в DI-композиции, см. [legacy.md](../legacy.md)). Для новых подписок используйте классы `Subscription<,>`.

## Отправка по расписанию

Джоб `ISendNotificationsJob` (`Framework.Configuration.BLL.Jobs`) вызывает `DomainObjectModification.ProcessAsync(1000, ct)`, то есть обрабатывает накопленные изменения доменных объектов пачками по 1000 штук. В хосте он регистрируется с `Cron.Never()`: запускается вручную или по расписанию из `Hangfire:JobTimings` (см. [jobs.md](jobs.md)).

## Тесты

- Юнит-тесты: `src/Notification/Framework.Notification.Tests`.
- Интеграционные: `SubscriptionCustomNotPersistentModelTests`, `NotificationCountryTests`, `GetNotificationPrincipalsByHierarchicalContextTests`, а также каталог `Subscriptions_Metadata` в `SampleSystem.IntegrationTests`.

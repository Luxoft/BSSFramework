# Аудит данных

Фреймворк поддерживает два независимых механизма аудита. Они решают разные задачи, и их можно использовать вместе.

| | Inline-аудит | Envers-аудит (история версий) |
|---|---|---|
| Что хранит | Кто и когда создал / изменил объект: поля прямо в строке сущности | Полную историю изменений: копия строки на каждую ревизию |
| Где хранится | Колонки той же таблицы (`CreateDate`, `CreatedBy`, `ModifyDate`, `ModifiedBy`) | Отдельные таблицы аудита и таблица ревизий, отдельный `DbContext` / схема |
| Читается | Как обычные поля | Через audit-контроллеры (`mainAuditApi/…`) и `Audit`-DTO |
| ORM | NHibernate и EF | NHibernate (`NHibernate.Envers`) и EF (собственная реализация `Framework.Database.EntityFramework.EnversAudit`) |

## Inline-аудит

### Модель

Сущность наследует `AuditPersistentDomainObjectBase`. Он добавляет поля `CreateDate`, `CreatedBy`, `ModifyDate`, `ModifiedBy`, `Active` и `Version` (`IVersionObject<long>`). Сеттеры `protected internal`: прикладной код их не заполняет, это делает фреймворк при сохранении.

### Настройка

Настройка находится в `SampleSystem.ServiceEnvironment/DependencyInjection/SampleSystemInlineAuditSetup.cs`:

```csharp
public static IInlineAuditSetup AddSampleSystemInlineAudit(this IInlineAuditSetup rootSetup) =>
    rootSetup
        .AddAuthorizationInlineAudit()
        .AddConfigurationInlineAudit()
        .AddDefault<SampleSystem.Domain.AuditPersistentDomainObjectBase>(
            v => v.CreateDate, v => v.CreatedBy, v => v.ModifyDate, v => v.ModifiedBy)
        .For<SampleSystem.Domain.ExtendedInlineAuditObj>(innerSetup =>
        {
            innerSetup.Add<Employee?, CurrentUserAuditValueResolver<Employee?>>(v => v.CreatedByEmployee, InlineAuditAction.Create)
                      .Add<Employee?, CurrentUserAuditValueResolver<Employee?>>(v => v.ModifiedByEmployee, InlineAuditAction.Modify);
        });
```

- `AddDefault<TBase>(createDate, createdBy, modifyDate, modifiedBy)` задаёт стандартные поля для всех наследников базового класса.
- `For<T>(...)` добавляет к конкретному типу собственные поля с резолверами значений.
- `InlineAuditAction` (`Create`, `Modify`) определяет момент, когда поле заполняется.
- Значение поля вычисляет `IAuditValueResolver<T>`. Готовые реализации: `NowAuditValueResolver` (текущее время), `CurrentUserAuditValueResolver` (текущий пользователь); можно написать свой.

### Подключение к ORM

В EF (`SampleSystemEntityFrameworkExtension`):

```csharp
options.AddInlineAudit(s => s.SetSetupType<SampleSystemInlineAuditSetup>())
```

Тип `SampleSystemInlineAuditSetup` реализует `IInlineAuditExtensionInnerSetup` и вызывает `AddSampleSystemInlineAudit()`. В NHibernate (`SampleSystemNHibernateExtension`) то же самое: `.AddInlineAudit(rootSetup => rootSetup.AddSampleSystemInlineAudit())`.

## Envers-аудит

Хранит версии каждого изменения: запись ревизии (кто, когда) и копию изменённых данных в таблицах аудита.

### Что аудируется

- В EF это все сущности, кроме проекций и типов с `[NotAuditedClass]`:

```csharp
setup.SetFilter(et => !et.ClrType.IsProjection() && !et.ClrType.HasAttribute<NotAuditedClassAttribute>());
```

- Тип аудита в SampleSystem описан в `SampleSystem.AuditDomain` (`BusinessUnitAudit`, `SampleSystemAuditRevisionEntity`), а маппинг сгенерирован (`Mapping/Envers`).
- Исключить сущность из аудита: `[NotAuditedClass]` из `Framework.Database.Mapping`.

### Подключение (EF)

```csharp
.AddDbContext<SampleSystemDbContext>((sp, options) => options
    .UseSqlServer(...)
    .AddEnversAudit(s => s.SetSetupType<SampleSystemEnversAuditSetup>()))

.AddDbContext<SampleSystemEnversAuditDbContext>(...)       // отдельный контекст для чтения аудита

.AddEntityFramework<SampleSystemDbContext>(s => s.AddEnversAudit()
                                                 .AddSecondaryContext<SampleSystemEnversAuditDbContext>());
```

Контекст `SampleSystemDbContext` реализует `IEnversAuditDbContext`, а `SampleSystemEnversAuditDbContext` содержит конфигурации таблиц аудита (`BusinessUnitAuditMap`, `SampleSystemAuditRevisionEntityMap`).

### Чтение истории

Для чтения используются audit-контроллеры и audit-DTO, которые генерируются отдельно (`AuditDTOGeneratorConfiguration`, `AuditServiceGeneratorConfiguration`, маршрут `mainAuditApi/[controller]/[action]`, см. [codegeneration.md](../codegeneration.md)). В коде чтение выполняет `IEfAuditReader` (EF).

### Схема БД

Таблицы аудита создаются при генерации схемы (для NHibernate: `AuditDBGenerator/AuditDatabaseScriptGenerator`). Для миграций и сравнения схем см. раздел «Схема БД» в [codegeneration.md](../codegeneration.md).

## Tracking

`Framework.Tracking` (контракты в `Framework.Tracking.Abstractions`: `ITrackingService<T>`, `TrackingResult<T>`, `TrackingProperty`, `GetChangesMode`) определяет, какие персистентные свойства объекта изменились в текущей сессии: имя свойства, старое и новое значение. Используйте его в BLL и валидаторах, когда нужно знать, что именно поменялось, без обращения к аудиту. Тесты находятся в `Framework.Tracking.Tests`.

## Тесты

Интеграционные тесты: `AuditTests`, `EnversTests`, `EnversBug1676`, `InlineAuditTests` (`SampleSystem.IntegrationTests`).

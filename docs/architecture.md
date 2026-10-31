# Архитектура

BSS Framework — набор NuGet-пакетов (`Luxoft.<ИмяПроекта>`) для построения бизнес-приложений на .NET 10. Приложение описывает **доменную модель** и **метаданные** (атрибуты, правила безопасности, подписки). Всё остальное — слой BLL, DTO, маппинг ORM, WebApi-контроллеры, схему БД — фреймворк генерирует или предоставляет готовым (см. [codegeneration.md](codegeneration.md)).

Поддерживаются два ORM: **NHibernate** и **Entity Framework**. Одна и та же доменная модель и один и тот же набор интеграционных тестов работают на обоих.

## Внешние зависимости фреймворка

Часть функциональности вынесена в отдельные пакеты, которых нет в этом репозитории:

| Пакеты | Что даёт |
|---|---|
| `Anch.*` (версия в `Directory.Packages.props`, свойство `Anch`) | Ядро (`Anch.Core`), **система безопасности** (`Anch.SecuritySystem*`: роли, контексты, права, виртуальные разрешения, уведомления по ролям), `GenericQueryable`, `GenericRepository`, `OData`, `HierarchicalExpand`, инфраструктуру тестов БД (`Anch.Testing.*`) |
| `Luxoft.Bss.Platform.*` | Логирование, документация API, middleware, интеграционные события |
| Hangfire, FluentMigrator, FluentNHibernate, NHibernate.Envers, EF Core, MediatR, FluentValidation | Фоновые задачи, миграции, ORM, аудит, прочее |

Поэтому описание Role&Context-безопасности в этом репозитории ограничивается подключением и настройкой (см. [security.md](subsystems/security.md)). Детали работы `Anch.SecuritySystem` смотрите в документации этого пакета.

## Слои и папки

Код в `src/` сгруппирован **по доменам**. Внутри домена используется единое деление:

```
*.Abstractions   контракты (интерфейсы, атрибуты)
*.Domain         модели и DTO самой подсистемы
*.Runtime        реализация
*.Tests          тесты
```

Расширения подключаются через DI: контракты лежат в `*.Abstractions`, реализации в `*.Runtime`, регистрация в `ServiceCollectionExtensions` / `Framework.DependencyInjection`.

| Папка | Назначение |
|---|---|
| `Framework.Core` | Слой 0: расширения, рефлексия, выражения (`Expression`), Maybe / TryResult, Period / Range, сериализация. Ни от чего не зависит |
| `Framework.DependencyInjection` | Хелперы регистрации сервисов |
| `Framework.ExpressionParsers` | Парсеры выражений (C# и «нативный») |
| `Domain` (`Framework.Relations`, `Framework.Restriction`) | Атрибуты и метаданные доменной модели: `IMaster` / `IDetail`, `[UniqueGroup]`, `[Required]`, `[MaxLength]` |
| `Validation` | Валидаторы и атрибуты валидации (см. [validation.md](subsystems/validation.md)) |
| `Database` | Абстракции БД; `*.NHibernate`, `*.EntityFramework`; аудит (`EnversAudit`, `InlineAudit`); `Tracking`; `DBGenerator`, `DALGenerator`; `SqlMapper`. Подпроекты `*.Legacy` — старое API (см. [legacy.md](legacy.md)) |
| `BLL` | Слой бизнес-логики: `BLL.Abstractions/Domain/Runtime/Events/Validation`, `BLL.DTOMapping` |
| `Application` | Прикладной слой: репозитории, `IServiceEvaluator`, джобы, именованные блокировки, события |
| `Infrastructure` | Hangfire, Swagger, DI-композиция (`AddBssFramework`), middleware, DAL-листенеры |
| `Notification`, `Subscriptions` | Отправка уведомлений и подписки на изменения объектов (см. [notification.md](subsystems/notification.md)) |
| `Projection`, `ExtendedMetadata` | Проекции доменных объектов и расширенные атрибуты метаданных |
| `Configurator` | Конфигуратор (`ApplicationVariable`, события) |
| `AutomationCore` | Инфраструктура интеграционных тестов (см. [integrationtests.md](integrationtests.md)) |
| `CodeGeneration` | Генераторы кода: `CodeDom`, `FileGeneration`, `DTOGenerator*`, `BLLGenerator`, `BLLCoreGenerator`, `ServiceModelGenerator`, `WebApiGenerator`, `ProjectionGenerator` |
| `_Authorization`, `_Configuration` | Готовые подсистемы авторизации и конфигурации: домен, BLL, сгенерированные DAL / DTO, WebApi |
| `_SampleSystem` | Референсное приложение и все интеграционные тесты |

Префикс `_` означает «прикладная система, собранная на фреймворке». Эти системы служат и потребителями кодогенерации.

## Анатомия приложения на фреймворке

Типичная цепочка проектов на примере `SampleSystem`:

```
SampleSystem.Domain                      доменные сущности + атрибуты
        │
        ├─ SampleSystem.Domain.Projections / .ManualProjections / .LegacyProjections
        │
SampleSystem.Generated.DAL.{NHibernate|EntityFramework}   маппинг ORM (генерируется)
SampleSystem.BLL.Core / SampleSystem.BLL                  бизнес-логика (генерируется + ручной код)
SampleSystem.Generated.DTO                                DTO и маппинг-сервисы (генерируется)
SampleSystem.Security(.Abstract)                          роли, контексты, правила безопасности
SampleSystem.Validation                                   правила валидации
SampleSystem.EventMetadata / .Events                      метаданные событий
SampleSystem.Subscriptions.Metadata                       подписки на уведомления
SampleSystem.WebApiCore                                   контроллеры (генерируются) + GenericProgram
SampleSystem.ServiceEnvironment                           общая DI-композиция
SampleSystem.ServiceEnvironment.{NHibernate|EntityFramework}   выбор ORM
SampleSystem.WebApiCore.{NHibernate|EntityFramework}      исполняемый хост (Program.cs)
```

### Выбор ORM

ORM-специфичные части сведены к двум расширениям `IBssFrameworkExtension`, остальное приложение от ORM не зависит:

- `SampleSystemEntityFrameworkExtension` регистрирует `SampleSystemDbContext` (и отдельный `SampleSystemEnversAuditDbContext` для аудита), подключает `UseGenericQueryable`, ленивые прокси, Envers- и Inline-аудит;
- аналог для NHibernate лежит в `SampleSystem.ServiceEnvironment.NHibernate`.

Хост (`SampleSystem.WebApiCore.EntityFramework/Program.cs`) передаёт нужное расширение в `GenericProgram.Main(args, extension)`.

### DI-композиция

Вся настройка собирается в `AddGeneralDependencyInjection` (`SampleSystem.ServiceEnvironment/DependencyInjection/`). Он вызывает `AddBssFramework` и цепочкой подключает:

- систему безопасности (`AddSecuritySystem`: контексты, роли, правила, источник пользователей, виртуальные права);
- именованные блокировки (`AddNamedLocks`);
- метаданные доменных событий (`SetDomainObjectEventMetadata`) и DAL-листенеры (`AddListener<…>`);
- подписки и уведомления (`AddSubscriptions`, `AddLegacyNotification`);
- BLL-контекст (`AddBLLSystem<ISampleSystemBLLContext, SampleSystemBLLContext>`);
- маппинг DTO (`SetDTOMapping`);
- прикладные сервисы (`AddGeneralApplicationServices`).

Блоки, помеченные в коде комментарием `// Legacy`, относятся к старому API (см. [legacy.md](legacy.md)).

Хост вызывает `AddGeneralDependencyInjection` один раз. Тесты делают то же самое: так проверяется ровно та композиция, что работает в продакшене.

### Поток запроса

1. Запрос приходит в сгенерированный контроллер (`Controllers/_Generated`). Маршруты: `Main`, `MainQuery` (`mainQueryApi/…`), `Integration` (`integrationApi/…`), `Audit` (`mainAuditApi/…`), `Auth` (`authApi/…`), `Config` (`configApi/…`).
2. Контроллер выполняет метод через `IServiceEvaluator` в рамках сессии БД и транзакции (`DBSessionEvaluator`).
3. BLL применяет правила безопасности (`ISecurityContext`, роли и операции, фильтрация запросов), валидацию и вызывает DAL.
4. DAL-листенеры (`IDALListener`) реагируют на изменения: аудит, события, подписки.
5. Результат преобразуется в DTO сервисом маппинга (`ISampleSystemDTOMappingService`).

### Типовые подсистемы приложения

| Подсистема | Где настраивается | Документ |
|---|---|---|
| Безопасность | `SampleSystem.Security`, `AddSecuritySystem` | [security.md](subsystems/security.md) |
| Валидация | атрибуты на модели, `SampleSystem.Validation` | [validation.md](subsystems/validation.md) |
| Аудит | `SampleSystemInlineAuditSetup`, `AddEnversAudit` | [audit.md](subsystems/audit.md) |
| Уведомления и подписки | `SampleSystem.Subscriptions.Metadata`, `AddNotification` | [notification.md](subsystems/notification.md) |
| Фоновые задачи | `IJob`, `Framework.Infrastructure.Hangfire` | [jobs.md](subsystems/jobs.md) |

## Принципы

- **Модель первична.** Новая сущность описывается один раз в `Domain`; DAL, DTO, BLL и контроллеры появляются после генерации.
- **Сгенерированное не правится руками.** Меняется генератор или модель, затем выполняется регенерация. CI проверяет актуальность (`CheckGenTests`).
- **ORM заменяем.** Прикладной код не должен зависеть от EF или NHibernate напрямую.
- **Расширение через DI.** Новое поведение подключается регистрацией в `IBssFrameworkSetup`, а не наследованием внутренних классов.
- **Nullable включён везде**, предупреждений ноль (см. [CONTRIBUTING.md](../CONTRIBUTING.md)).

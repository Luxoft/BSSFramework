# Фоновые задачи (Jobs)

Фоновые задачи построены на [Hangfire](https://www.hangfire.io/) с хранилищем в SQL Server. Фреймворк добавляет к нему DI, выполнение от имени заданного пользователя, расписание из конфигурации и единый контракт `IJob`.

Проекты: `src/Application/Framework.Application.Abstractions/Jobs` (контракты), `src/Infrastructure/Framework.Infrastructure.Hangfire` (интеграция).

## Контракт

```csharp
public interface IJob
{
    Task ExecuteAsync(CancellationToken ct);
}
```

Пример из `SampleSystem.ServiceEnvironment/Jobs/SampleJob.cs`:

```csharp
[DisableConcurrentExecution(timeoutInSeconds: 1000)]
public class SampleJob(
    [DisabledSecurity] IRepository<TestJobObject> testRepository,
    ILogger<SampleJob> logger,
    ICurrentUser currentUser) : IJob
{
    public async Task ExecuteAsync(CancellationToken ct)
    {
        logger.LogInformation("Job executed");
        await testRepository.SaveAsync(new TestJobObject(), ct);
    }
}
```

- Зависимости внедряются через конструктор, как у любого сервиса.
- Атрибуты Hangfire (`[DisableConcurrentExecution]`) работают как обычно.
- `[DisabledSecurity]` отключает проверку прав в репозитории: для системных задач это обычно нужно (см. [security.md](security.md)).
- Задача выполняется через `MiddlewareJob` и `IJobServiceEvaluator`; пользователь для проверки прав задаётся `JobImpersonateData` (настройка `RunAs`).

## Регистрация

Регистрация идёт в хосте (`GenericProgram.Main`):

```csharp
builder.Services.AddHangfireBss(
    builder.Configuration,
    s => s.AddJob<SampleJob>(new JobSettings { DisplayName = "SampleDisplayName" })
          .AddJob<ISendNotificationsJob>((job, ct) => job.ExecuteAsync(ct), new JobSettings { CronTiming = Cron.Never() }));
```

`JobSettings`:

| Параметр | Смысл |
|---|---|
| `Name` | Техническое имя джоба и ключ расписания в конфигурации. По умолчанию выводится из имени типа: у интерфейса отбрасывается префикс `I`, у всех отбрасывается суффикс `Job` (`SampleJob` → `Sample`, `ISendNotificationsJob` → `SendNotifications`) |
| `DisplayName` | Отображаемое имя (в Hangfire Dashboard). По умолчанию имя типа |
| `CronTiming` | Cron-выражение; `Cron.Never()` — только ручной запуск. Если не задано, берётся из `Hangfire:JobTimings` по `Name`; если и там нет, при регистрации выбрасывается исключение |

Джоб можно зарегистрировать как класс (`AddJob<SampleJob>`) или как интерфейс с лямбдой вызова (`AddJob<ISendNotificationsJob>((job, ct) => job.ExecuteAsync(ct), …)`). Для классов фреймворк сам регистрирует тип в DI (scoped).

Для тестов джобы регистрируются из сборки целиком: `AddJobs([typeof(SampleJob).Assembly])`, а пользователь джоба задаётся через `JobImpersonateData` (`.AddSingleton(new JobImpersonateData("sampleSystemTestJob"))`).

## Конфигурация

Секция `Hangfire` в `appsettings.json` (`BssHangfireSettings`):

```json
"Hangfire": {
  "RunAs": "testCred",
  "Enabled": false,
  "JobTimings": [
    { "Name": "Sample", "Schedule": "0/1 * * * *" }
  ]
}
```

| Параметр | Смысл |
|---|---|
| `Enabled` | Включает Hangfire. При `false` не регистрируются ни сервер Hangfire, ни джобы, ни `JobImpersonateData` |
| `RunAs` | Логин пользователя, от имени которого выполняются задачи (регистрируется как `JobImpersonateData`) |
| `JobTimings` | Расписание задач: `Name` джоба (см. выше) и cron-выражение `Schedule` |

Рекуррентные задачи регистрируются в Hangfire при старте (`RunJobs`: `AddOrUpdate(name, job, cron)`). Расписание из конфигурации применяется при каждом запуске приложения.

Хранилище использует строку подключения `DefaultConnection`; имя можно поменять через `SetConnectionStringName`.

## Dashboard

Доступ к Hangfire Dashboard ограничен администраторами (`AdminHangfireAuthorization`): проверка идёт через систему безопасности для текущего пользователя без учёта run-as.

## Именованные блокировки

Для взаимного исключения на уровне данных (а не процессов) используются именованные блокировки: `INamedLockService.LockAsync(namedLock, lockRole, ct)`. В SampleSystem они регистрируются в `AddNamedLocks` (в том числе вручную для `BusinessUnitAncestorLink`, `ManagementUnitAncestorLink`, `LocationAncestorLink`).

## Тесты

`JobTests` в `SampleSystem.IntegrationTests`.

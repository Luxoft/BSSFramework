# Background jobs (Jobs)

Background jobs are built on [Hangfire](https://www.hangfire.io/) with SQL Server storage. The framework adds DI, execution on behalf of a given user, a schedule from configuration, and a single `IJob` contract.

Projects: `src/Application/Framework.Application.Abstractions/Jobs` (contracts), `src/Infrastructure/Framework.Infrastructure.Hangfire` (integration).

## Contract

```csharp
public interface IJob
{
    Task ExecuteAsync(CancellationToken ct);
}
```

Example from `SampleSystem.ServiceEnvironment/Jobs/SampleJob.cs`:

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

- Dependencies are injected via the constructor, like for any service.
- Hangfire attributes (`[DisableConcurrentExecution]`) work as usual.
- `[DisabledSecurity]` disables permission checks in the repository: system jobs usually need this (see [security.md](security.md)).
- A job is executed via `MiddlewareJob` and `IJobServiceEvaluator`; the user for permission checks is set by `JobImpersonateData` (the `RunAs` setting).

## Registration

Registration happens in the host (`GenericProgram.Main`):

```csharp
builder.Services.AddHangfireBss(
    builder.Configuration,
    s => s.AddJob<SampleJob>(new JobSettings { DisplayName = "SampleDisplayName" })
          .AddJob<ISendNotificationsJob>((job, ct) => job.ExecuteAsync(ct), new JobSettings { CronTiming = Cron.Never() }));
```

`JobSettings`:

| Parameter | Meaning |
|---|---|
| `Name` | Technical job name and the schedule key in configuration. By default derived from the type name: for an interface the `I` prefix is dropped, and for all types the `Job` suffix is dropped (`SampleJob` → `Sample`, `ISendNotificationsJob` → `SendNotifications`) |
| `DisplayName` | Display name (in the Hangfire Dashboard). Defaults to the type name |
| `CronTiming` | Cron expression; `Cron.Never()` means manual run only. If not set, it is taken from `Hangfire:JobTimings` by `Name`; if it is not there either, an exception is thrown at registration |

A job can be registered as a class (`AddJob<SampleJob>`) or as an interface with an invocation lambda (`AddJob<ISendNotificationsJob>((job, ct) => job.ExecuteAsync(ct), …)`). For classes the framework registers the type in DI itself (scoped).

For tests, jobs are registered from a whole assembly: `AddJobs([typeof(SampleJob).Assembly])`, and the job user is set via `JobImpersonateData` (`.AddSingleton(new JobImpersonateData("sampleSystemTestJob"))`).

## Configuration

The `Hangfire` section in `appsettings.json` (`BssHangfireSettings`):

```json
"Hangfire": {
  "RunAs": "testCred",
  "Enabled": false,
  "JobTimings": [
    { "Name": "Sample", "Schedule": "0/1 * * * *" }
  ]
}
```

| Parameter | Meaning |
|---|---|
| `Enabled` | Enables Hangfire. When `false`, neither the Hangfire server, nor the jobs, nor `JobImpersonateData` are registered |
| `RunAs` | Login of the user on whose behalf jobs run (registered as `JobImpersonateData`) |
| `JobTimings` | Job schedule: the job `Name` (see above) and the cron expression `Schedule` |

Recurring jobs are registered in Hangfire at startup (`RunJobs`: `AddOrUpdate(name, job, cron)`). The schedule from configuration is applied on every application start.

The storage uses the `DefaultConnection` connection string; the name can be changed via `SetConnectionStringName`.

## Dashboard

Access to the Hangfire Dashboard is restricted to administrators (`AdminHangfireAuthorization`): the check goes through the security system for the current user, ignoring run-as.

## Named locks

For mutual exclusion at the data level (rather than between processes), named locks are used: `INamedLockService.LockAsync(namedLock, lockRole, ct)`. In SampleSystem they are registered in `AddNamedLocks` (including manually for `BusinessUnitAncestorLink`, `ManagementUnitAncestorLink`, `LocationAncestorLink`).

## Tests

`JobTests` in `SampleSystem.IntegrationTests`.

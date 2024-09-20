using Framework.DomainDriven.Jobs;

using Hangfire;
using Hangfire.SqlServer;

namespace Framework.HangfireCore;

public interface IBssHangfireSettings
{
    IBssHangfireSettings SetConnectionString(string connectionString);

    IBssHangfireSettings SetConnectionStringName(string connectionStringName);

    IBssHangfireSettings WithGlobalConfiguration(Action<IGlobalConfiguration> globalConfigurationAction);

    IBssHangfireSettings WithSqlServerStorageOptions(Action<SqlServerStorageOptions> setupOptions);

    IBssHangfireSettings AddJob<TJob>(JobSettings? jobSettings = null)
        where TJob : IJob =>
        this.AddJob<TJob, CancellationToken>((job, cancellationToken) => job.ExecuteAsync(cancellationToken), jobSettings);

    IBssHangfireSettings AddJob<TJob>(Func<TJob, CancellationToken, Task> executeAction, JobSettings? jobSettings = null) =>
        this.AddJob<TJob, CancellationToken>(executeAction, jobSettings);

    IBssHangfireSettings AddJob<TJob, TArg>(Func<TJob, TArg, Task> executeAction, JobSettings? jobSettings = null);
}

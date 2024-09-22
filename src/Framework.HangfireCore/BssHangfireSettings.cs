using Framework.Core;
using Framework.DomainDriven.Jobs;

using Hangfire;
using Hangfire.Common;
using Hangfire.SqlServer;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.HangfireCore;

public class BssHangfireSettings : IBssHangfireSettings
{
    private Func<IConfiguration, string> getConnectionStringFunc = default!;

    private readonly List<Action<IServiceCollection>> registerActions = [];

    private readonly List<Action> runJobActions = [];

    private bool autoRegisterJob = true;

    private IJobNameExtractPolicy jobNameExtractPolicy = new JobNameExtractPolicy();

    private readonly SqlServerStorageOptions sqlServerStorageOptions = new()
    {
        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
        QueuePollInterval = TimeSpan.Zero,
        UseRecommendedIsolationLevel = true,
        UsePageLocksOnDequeue = true,
        DisableGlobalLocks = true
    };

    private Action<IGlobalConfiguration> globalConfigurationAction = _ => { };

    public bool Enabled { get; set; }

    public string? RunAs { get; set; }

    public JobTiming[] JobTimings { get; set; } = [];


    public BssHangfireSettings()
    {
        this.SetConnectionStringName("DefaultConnectionString");
    }

    public IBssHangfireSettings SetJobNameExtractPolicy(IJobNameExtractPolicy policy)
    {
        this.jobNameExtractPolicy = policy;

        return this;
    }

    public IBssHangfireSettings RegisterRegisterAsServices(bool enabled)
    {
        this.autoRegisterJob = enabled;

        return this;
    }

    public IBssHangfireSettings SetConnectionString(string newConnectionString)
    {
        this.getConnectionStringFunc = _ => newConnectionString;

        return this;
    }

    public IBssHangfireSettings SetConnectionStringName(string newConnectionStringName)
    {
        this.getConnectionStringFunc = configuration => configuration.GetConnectionString("DefaultConnection")!;

        return this;
    }

    public IBssHangfireSettings WithGlobalConfiguration(Action<IGlobalConfiguration> newGlobalConfigurationAction)
    {
        var prevAction = this.globalConfigurationAction;

        this.globalConfigurationAction = cfg =>
        {
            prevAction(cfg);
            newGlobalConfigurationAction(cfg);
        };

        return this;
    }

    public IBssHangfireSettings WithSqlServerStorageOptions(Action<SqlServerStorageOptions> setupOptions)
    {
        setupOptions(this.sqlServerStorageOptions);

        return this;
    }


    public IBssHangfireSettings AddJob<TJob, TArg>(Func<TJob, TArg, Task> executeAction, JobSettings? jobSettings = null)
        where TJob : class
    {
        this.registerActions.Add(services =>
                                 {
                                     if (!typeof(TJob).IsInterface && this.autoRegisterJob)
                                     {
                                         services.AddScoped<TJob>();
                                     }

                                     services.AddSingleton(new JobInfo<TJob, TArg>(executeAction));
                                 });

        this.runJobActions.Add(
            () =>
            {
                var jobName = jobSettings?.Name ?? this.jobNameExtractPolicy.GetName(typeof(TJob));

                var cronTiming = jobSettings?.CronTiming
                                 ?? this.JobTimings.Where(jt => jt.Name == jobName).Select(jt => jt.Schedule).SingleOrDefault()
                                 ?? throw new Exception($"{nameof(JobTiming)} for job '{jobName}' not found");

                RecurringJob.AddOrUpdate<MiddlewareJob<TJob, TArg>>(jobName, job => job.ExecuteAsync(default!), cronTiming);
            });

        return this;
    }

    public void Initialize(IServiceCollection services, IConfiguration configuration)
    {
        if (this.RunAs != null)
        {
            services.AddSingleton(new JobImpersonateData(this.RunAs));
        }

        this.registerActions.ForEach(a => a(services));

        services.AddSingleton(this);

        services.AddHangfire(
            z =>
            {
                z.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                 .UseSimpleAssemblyNameTypeSerializer()
                 .UseRecommendedSerializerSettings()
                 .UseSerilogLogProvider()
                 .UseSqlServerStorage(() => new SqlConnection(this.getConnectionStringFunc(configuration)), this.sqlServerStorageOptions)
                 .Self(this.globalConfigurationAction);
            });

        services.AddHangfireServer();
    }

    public void RunJobs()
    {
        JobFilterProviders.Providers.RemoveBy(provider => provider is JobFilterAttributeFilterProvider);
        JobFilterProviders.Providers.Add(new BssJobFilterAttributeFilterProvider());

        this.runJobActions.ForEach(a => a());
    }
}

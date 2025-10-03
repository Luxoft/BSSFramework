using System.Reflection;

using CommonFramework;

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

    private readonly List<Action<IServiceCollection>> registerServicesActions = [];

    private readonly Dictionary<Job, JobSettings> registerJobs = [];

    private readonly Dictionary<MethodInfo, string> jobNames = [];

    private bool autoRegisterJob = true;

    private IJobNameExtractPolicy jobNameExtractPolicy = new JobNameExtractPolicy();

    private readonly SqlServerStorageOptions sqlServerStorageOptions = new()
                                                                       {
                                                                           CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                                                                           SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                                                                           QueuePollInterval = TimeSpan.Zero,
                                                                           UseRecommendedIsolationLevel = true,
                                                                           UsePageLocksOnDequeue = true,
                                                                           DisableGlobalLocks = true,
                                                                       };

    private Action<IGlobalConfiguration> globalConfigurationAction = _ => { };

    public bool Enabled { get; set; }

    public string? RunAs { get; set; }

    public JobTiming[] JobTimings { get; set; } = [];

    public string GetDisplayName(Job job)
    {
        return this.jobNames[job.Method];
    }


    public BssHangfireSettings()
    {
        this.SetConnectionStringName("DefaultConnection");
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

    public IBssHangfireSettings SetConnectionString(string connectionString)
    {
        this.getConnectionStringFunc = _ => connectionString;

        return this;
    }

    public IBssHangfireSettings SetConnectionStringName(string connectionStringName)
    {
        this.getConnectionStringFunc = configuration => configuration.GetConnectionString(connectionStringName)!;

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
        this.registerServicesActions.Add(
            services =>
            {
                if (!typeof(TJob).IsInterface && this.autoRegisterJob)
                {
                    services.AddScoped<TJob>();
                }

                services.AddSingleton(new JobInfo<TJob, TArg>(executeAction));

                var jobName = jobSettings?.Name ?? this.jobNameExtractPolicy.GetName(typeof(TJob));

                var cronTiming = jobSettings?.CronTiming
                                 ?? this.JobTimings.Where(jt => jt.Name == jobName).Select(jt => jt.Schedule).SingleOrDefault()
                                 ?? throw new Exception($"{nameof(JobTiming)} for job '{jobName}' not found");

                var job = Job.FromExpression(ExpressionHelper.Create((MiddlewareJob<TJob, TArg> job) => job.ExecuteAsync(default!)));

                var actualSettings = new JobSettings
                                     {
                                         Name = jobName,
                                         CronTiming = cronTiming,
                                         DisplayName = jobSettings?.DisplayName ?? this.jobNameExtractPolicy.GetDisplayName(typeof(TJob))
                                     };

                this.registerJobs.Add(job, actualSettings);

                this.jobNames.Add(job.Method, actualSettings.DisplayName);
            });

        return this;
    }

    public void Initialize(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(this);

        if (!this.Enabled)
        {
            return;
        }

        if (this.RunAs != null)
        {
            services.AddSingleton(new JobImpersonateData(this.RunAs));
        }

        this.registerServicesActions.ForEach(a => a(services));

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

    public void RunJobs(IServiceProvider serviceProvider)
    {
        JobFilterProviders.Providers.RemoveBy(provider => provider is JobFilterAttributeFilterProvider);
        JobFilterProviders.Providers.Add(new BssJobFilterAttributeFilterProvider());

        var manager = serviceProvider.GetRequiredService<IRecurringJobManager>();

        foreach (var pair in this.registerJobs)
        {
            manager.AddOrUpdate(pair.Value.Name, pair.Key, pair.Value.CronTiming);
        }
    }
}

using Framework.Authorization.ApproveWorkflow;
using Framework.Authorization.BLL;
using Framework.Authorization.Generated.DAL.NHibernate;
using Framework.Cap;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Generated.DAL.NHibernate;
using Framework.Core.Services;
using Framework.DependencyInjection;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.NHibernate;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using nuSpec.Abstraction;
using nuSpec.NHibernate;

using SampleSystem.BLL;
using SampleSystem.Generated.DAL.NHibernate;
using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore.CustomReports;
using SampleSystem.WebApiCore.Env.Database;

using UserAuthenticationService = SampleSystem.WebApiCore.Env.UserAuthenticationService;

namespace SampleSystem.WebApiCore
{
    public static class EnvironmentExtensions
    {
        public static IServiceCollection AddEnvironment(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services
                .AddHttpContextAccessor();

            services.AddDatabaseSettings(connectionString);
            services.AddCapBss(connectionString);

            // Notifications
            services
                .AddSingleton<ISubscriptionMetadataFinder, SampleSystemSubscriptionsMetadataFinder>();
            services.RegisterMessageSenderDependencies<SampleSystemServiceEnvironment, ISampleSystemBLLContext>(configuration);
            services.RegisterRewriteReceiversDependencies(configuration);

            // Others
            services.AddSingleton<IDateTimeService>(DateTimeService.Default);
            services.AddSingleton<IUserAuthenticationService, UserAuthenticationService>();
            services.AddSingleton<ISpecificationEvaluator, NhSpecificationEvaluator>();


            return services.AddSingleton<SampleSystemServiceEnvironment>()
                           .AddControllerEnvironment();
        }

        public static IServiceCollection AddDatabaseSettings(this IServiceCollection services, string connectionString) =>
                services
                        .AddSingleton<IDBSessionFactory, SampleSystemNHibSessionFactory>()
                        .AddSingleton<NHibConnectionSettings>()
                        .AddSingleton<IMappingSettings>(AuthorizationMappingSettings.CreateDefaultAudit(string.Empty))
                        .AddSingleton<IMappingSettings>(ConfigurationMappingSettings.CreateDefaultAudit(string.Empty))
                        .AddSingleton<IMappingSettings>(
                                                        new SampleSystemMappingSettings(
                                                                                        new DatabaseName(string.Empty, "app"),
                                                                                        connectionString));

        public static IServiceCollection AddControllerEnvironment(this IServiceCollection services)
        {
            services.AddSingleton<IExceptionProcessor, ApiControllerExceptionService<IServiceEnvironment<ISampleSystemBLLContext>, ISampleSystemBLLContext>>();

            services.AddSingleton<SampleSystemCustomReportsServiceEnvironment>();

            // Environment
            services
                .AddSingleton<IServiceEnvironment<ISampleSystemBLLContext>>(x => x.GetRequiredService<SampleSystemServiceEnvironment>())
                .AddSingleton<IServiceEnvironment<IAuthorizationBLLContext>>(x => x.GetRequiredService<SampleSystemServiceEnvironment>())
                .AddSingleton<IServiceEnvironment<IConfigurationBLLContext>>(x => x.GetRequiredService<SampleSystemServiceEnvironment>());

            return services;
        }

        public static IServiceCollection AddWorkflowCore(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                   .AddWorkflow(x => x.UseSqlServer(configuration["WorkflowCoreConnectionString"], true, true))
                   .AddLogging();
        }

        public static IServiceCollection AddAuthWorkflow(this IServiceCollection services)
        {
            return services
                   .AddScoped<IWorkflowApproveProcessor, WorkflowApproveProcessor>()
                   .AddScoped<IDALListener, PermissionWorkflowDALListener>()

                   .AddTransient<StartWorkflow>()
                   .AddTransient<PublishEvent>()
                   .AddTransient<SendFinalEvent>()

                   .AddTransient<CanAutoApproveStep>();
        }
    }
}

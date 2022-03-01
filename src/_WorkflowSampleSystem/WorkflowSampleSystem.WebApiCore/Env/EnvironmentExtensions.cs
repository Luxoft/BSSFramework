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

using WorkflowSampleSystem.BLL;
using WorkflowSampleSystem.Generated.DAL.NHibernate;
using WorkflowSampleSystem.ServiceEnvironment;
using WorkflowSampleSystem.WebApiCore.CustomReports;
using WorkflowSampleSystem.WebApiCore.Env.Database;

using UserAuthenticationService = WorkflowSampleSystem.WebApiCore.Env.UserAuthenticationService;

namespace WorkflowSampleSystem.WebApiCore
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
                .AddSingleton<ISubscriptionMetadataFinder, WorkflowSampleSystemSubscriptionsMetadataFinder>();
            services.RegisterMessageSenderDependencies<WorkflowSampleSystemServiceEnvironment, IWorkflowSampleSystemBLLContext>(configuration);
            services.RegisterRewriteReceiversDependencies(configuration);

            // Others
            services.AddSingleton<IDateTimeService>(DateTimeService.Default);
            services.AddSingleton<IUserAuthenticationService, UserAuthenticationService>();
            services.AddSingleton<ISpecificationEvaluator, NhSpecificationEvaluator>();


            return services.AddSingleton<WorkflowSampleSystemServiceEnvironment>()
                           .AddControllerEnvironment();
        }

        public static IServiceCollection AddDatabaseSettings(this IServiceCollection services, string connectionString) =>
                services
                        .AddSingleton<IDBSessionFactory, WorkflowSampleSystemNHibSessionFactory>()
                        .AddSingleton<NHibConnectionSettings>()
                        .AddSingleton<IMappingSettings>(AuthorizationMappingSettings.CreateDefaultAudit(string.Empty))
                        .AddSingleton<IMappingSettings>(ConfigurationMappingSettings.CreateDefaultAudit(string.Empty))
                        .AddSingleton<IMappingSettings>(
                                                        new WorkflowSampleSystemMappingSettings(
                                                                                        new DatabaseName(string.Empty, "app"),
                                                                                        connectionString));

        public static IServiceCollection AddControllerEnvironment(this IServiceCollection services)
        {
            services.AddSingleton<IExceptionProcessor, ApiControllerExceptionService<IServiceEnvironment<IWorkflowSampleSystemBLLContext>, IWorkflowSampleSystemBLLContext>>();

            services.AddSingleton<WorkflowSampleSystemCustomReportsServiceEnvironment>();

            // Environment
            services
                .AddSingleton<IServiceEnvironment<IWorkflowSampleSystemBLLContext>>(x => x.GetRequiredService<WorkflowSampleSystemServiceEnvironment>())
                .AddSingleton<IServiceEnvironment<IAuthorizationBLLContext>>(x => x.GetRequiredService<WorkflowSampleSystemServiceEnvironment>())
                .AddSingleton<IServiceEnvironment<IConfigurationBLLContext>>(x => x.GetRequiredService<WorkflowSampleSystemServiceEnvironment>());

            return services;
        }

    }
}

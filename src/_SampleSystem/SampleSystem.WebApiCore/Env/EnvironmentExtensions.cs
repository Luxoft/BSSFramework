using System;

using Framework.Authorization.BLL;
using Framework.Authorization.Generated.DAL.NHibernate;
using Framework.Cap;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Generated.DAL.NHibernate;
using Framework.Core.Services;
using Framework.DependencyInjection;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.NHibernate;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;

using JetBrains.Annotations;

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
            services.RegisterMessageSenderDependencies<ISampleSystemBLLContext>(configuration);
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
                        .AddScoped<INHibSessionSetup, NHibSessionSettings>()
                        .AddScoped<IDBSession, NHibSession>()

                        .AddSingleton<INHibSessionEnvironmentSettings, NHibSessionEnvironmentSettings>()
                        .AddSingleton<NHibConnectionSettings>()
                        .AddSingleton<NHibSessionEnvironment, SampleSystemNHibSessionEnvironment>()

                        .AddSingleton<IMappingSettings>(AuthorizationMappingSettings.CreateDefaultAudit(string.Empty))
                        .AddSingleton<IMappingSettings>(ConfigurationMappingSettings.CreateDefaultAudit(string.Empty))
                        .AddSingleton<IMappingSettings>(
                                                        new SampleSystemMappingSettings(
                                                                                        new DatabaseName(string.Empty, "app"),
                                                                                        connectionString));

        public static IServiceCollection AddControllerEnvironment(this IServiceCollection services)
        {
            services.AddSingleton<IExceptionProcessor, ApiControllerExceptionService<ISampleSystemBLLContext>>();

            services.AddSingleton<SampleSystemCustomReportsServiceEnvironment>();

            // Environment
            services.AddSingleton<SampleSystemServiceEnvironment>();

            services.AddSingleton<IContextEvaluator<IAuthorizationBLLContext>, ContextEvaluator<IAuthorizationBLLContext>>();
            services.AddSingleton<IContextEvaluator<ISampleSystemBLLContext>, ContextEvaluator<ISampleSystemBLLContext>>();

            return services;
        }

        public static IServiceCollection AddWorkflowCore([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return services.AddWorkflowCore(configuration["WorkflowCoreConnectionString"]);
        }

        public static IServiceCollection AddWorkflowCore([NotNull] this IServiceCollection services, [NotNull] string connectionString)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));

            return services
                   .AddWorkflow(x => x.UseSqlServer(connectionString, true, true))
                   .AddLogging();
        }
    }
}

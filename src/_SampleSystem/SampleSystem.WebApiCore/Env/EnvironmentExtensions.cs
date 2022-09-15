using System;

using Framework.Authorization.BLL;
using Framework.Authorization.Generated.DAL.NHibernate;
using Framework.Authorization.Generated.DTO;
using Framework.Cap;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Generated.DAL.NHibernate;
using Framework.Configuration.Generated.DTO;
using Framework.Core.Services;
using Framework.CustomReports.Domain;
using Framework.CustomReports.Services;
using Framework.DependencyInjection;
using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;
using Framework.DomainDriven.NHibernate.Audit;
using Framework.DomainDriven.Serialization;
using Framework.DomainDriven.SerializeMetadata;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;

using JetBrains.Annotations;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using nuSpec.Abstraction;
using nuSpec.NHibernate;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.Generated.DAL.NHibernate;
using SampleSystem.Generated.DTO;
using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore.CustomReports;
using SampleSystem.WebApiCore.Env;
using SampleSystem.WebApiCore.Env.Database;

namespace SampleSystem.WebApiCore
{
    public static class EnvironmentExtensions
    {
        public static IServiceCollection AddEnvironment(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddHttpContextAccessor();
            services.AddScoped<IWebApiDBSessionModeResolver, WebApiDBSessionModeResolver>();
            services.AddScoped<IWebApiCurrentMethodResolver, WebApiCurrentMethodResolver>();

            services.AddDatabaseSettings(connectionString);
            services.AddCapBss(connectionString);

            // Notifications
            services.AddSingleton<ISubscriptionMetadataFinder, SampleSystemSubscriptionsMetadataFinder>();
            services.RegisterMessageSenderDependencies<ISampleSystemBLLContext>(configuration);
            services.RegisterRewriteReceiversDependencies(configuration);

            // Others
            services.AddSingleton<IDateTimeService>(DateTimeService.Default);

            services.AddSingleton<SampleSystemDefaultUserAuthenticationService>();
            services.AddSingletonFrom<IDefaultUserAuthenticationService, SampleSystemDefaultUserAuthenticationService>();
            services.AddSingletonFrom<IAuditRevisionUserAuthenticationService, SampleSystemDefaultUserAuthenticationService>();

            services.AddScoped<SampleSystemUserAuthenticationService>();
            services.AddScopedFrom<IUserAuthenticationService, SampleSystemUserAuthenticationService>();
            services.AddScopedFrom<IImpersonateService, SampleSystemUserAuthenticationService>();

            services.AddSingleton<ISpecificationEvaluator, NhSpecificationEvaluator>();

            services.AddSingleton<WorkflowManager>();
            services.AddSingletonFrom<IWorkflowManager, WorkflowManager>();

            services.AddSingleton(new SubscriptionMetadataStore(new SampleSystemSubscriptionsMetadataFinder()));

            return services.AddControllerEnvironment();
        }

        public static IServiceCollection AddDatabaseSettings(this IServiceCollection services, string connectionString) =>

                services.AddDatabaseSettings(setupObj =>

                                                     setupObj.AddEventListener<DefaultDBSessionEventListener>()
                                                             .AddEventListener<SubscriptionDBSessionEventListener>()

                                                             .SetEnvironment<SampleSystemNHibSessionEnvironment>()

                                                             .AddMapping(AuthorizationMappingSettings.CreateDefaultAudit(string.Empty))
                                                             .AddMapping(ConfigurationMappingSettings.CreateDefaultAudit(string.Empty))
                                                             .AddMapping(new SampleSystemMappingSettings(new DatabaseName(string.Empty, "app"), connectionString)));

        public static IServiceCollection AddControllerEnvironment(this IServiceCollection services)
        {
            services.AddSingleton<IWebApiExceptionExpander, WebApiExceptionExpander>();

            services.AddSingleton<IReportParameterValueService<ISampleSystemBLLContext, PersistentDomainObjectBase, SampleSystemSecurityOperationCode>, ReportParameterValueService<ISampleSystemBLLContext, PersistentDomainObjectBase, SampleSystemSecurityOperationCode>>();
            services.AddSingleton<ISystemMetadataTypeBuilder>(new SystemMetadataTypeBuilder<PersistentDomainObjectBase>(DTORole.All, typeof(PersistentDomainObjectBase).Assembly));
            services.AddSingleton<SampleSystemCustomReportsServiceEnvironment>();
            services.AddSingletonFrom((SampleSystemCustomReportsServiceEnvironment env) => env.ReportService);
            services.AddSingleton<ISecurityOperationCodeProvider<SampleSystemSecurityOperationCode>, SecurityOperationCodeProvider>();

            services.AddSingleton<IDBSessionEvaluator, DBSessionEvaluator>();

            services.AddSingleton<IContextEvaluator<IAuthorizationBLLContext>, ContextEvaluator<IAuthorizationBLLContext>>();
            services.AddSingleton<IContextEvaluator<IConfigurationBLLContext>, ContextEvaluator<IConfigurationBLLContext>>();
            services.AddSingletonFrom<IContextEvaluator<Framework.DomainDriven.BLL.Configuration.IConfigurationBLLContext>, IContextEvaluator<IConfigurationBLLContext>>();
            services.AddSingleton<IContextEvaluator<ISampleSystemBLLContext>, ContextEvaluator<ISampleSystemBLLContext>>();

            services.AddScoped<IApiControllerBaseEvaluator<EvaluatedData<IAuthorizationBLLContext, IAuthorizationDTOMappingService>>, ApiControllerBaseSingleCallEvaluator<EvaluatedData<IAuthorizationBLLContext, IAuthorizationDTOMappingService>>>();
            services.AddScoped<IApiControllerBaseEvaluator<EvaluatedData<IConfigurationBLLContext, IConfigurationDTOMappingService>>, ApiControllerBaseSingleCallEvaluator<EvaluatedData<IConfigurationBLLContext, IConfigurationDTOMappingService>>>();
            services.AddScoped<IApiControllerBaseEvaluator<EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService>>, ApiControllerBaseSingleCallEvaluator<EvaluatedData<ISampleSystemBLLContext, ISampleSystemDTOMappingService>>>();

            return services;
        }

        public static IServiceCollection AddWorkflowCore([NotNull] this IServiceCollection services, [NotNull] IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return services.AddWorkflowCore(configuration.GetConnectionString("WorkflowCoreConnection"));
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

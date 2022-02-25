using System;
using Framework.Authorization.BLL;
using Framework.Authorization.Generated.DAL.NHibernate;
using Framework.Cap;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate;
using Framework.Core;
using Framework.Core.Services;
using Framework.DependencyInjection;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem.Rules.Builders;
using Framework.DomainDriven.NHibernate;
using Framework.DomainDriven.ServiceModel;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;
using Framework.NotificationCore.Services;
using Framework.NotificationCore.Settings;
using Framework.Workflow.Generated.DAL.NHibernate;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using nuSpec.Abstraction;
using nuSpec.NHibernate;

using SampleSystem.BLL;
using SampleSystem.Generated.DAL.NHibernate;
using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore.CustomReports;
using SampleSystem.WebApiCore.Env.Database;

using PersistentDomainObjectBase = SampleSystem.Domain.PersistentDomainObjectBase;
using UserAuthenticationService = SampleSystem.WebApiCore.Env.UserAuthenticationService;

namespace SampleSystem.WebApiCore
{
    public class CoreSampleSystemServiceEnvironment : SampleSystemServiceEnvironmentStandard
    {
        protected readonly SmtpSettings smtpSettings;

        protected readonly IRewriteReceiversService rewriteReceiversService;

        public CoreSampleSystemServiceEnvironment(
            IServiceProvider serviceProvider,
            IDBSessionFactory sessionFactory,
            INotificationContext notificationContext,
            IUserAuthenticationService userAuthenticationService,
            IOptions<SmtpSettings> smtpSettings,
            IRewriteReceiversService rewriteReceiversService = null,
            bool? isDebugMode = null,
            Func<ISampleSystemBLLContext, ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid>> securityExpressionBuilderFactoryFunc = null)
            : base(serviceProvider, sessionFactory, notificationContext, userAuthenticationService, isDebugMode, securityExpressionBuilderFactoryFunc)
        {
            this.smtpSettings = smtpSettings.Value;
            this.rewriteReceiversService = rewriteReceiversService;
        }

        protected override SampleSystemBLLContextContainerStandard CreateBLLContextContainer(IServiceProvider scopedServiceProvider, IDBSession session, string currentPrincipalName = null)
        {
            return new CoreSampleSystemBLLContextContainer(
                                                           this,
                                                           scopedServiceProvider,
                                                           this.DefaultAuthorizationValidatorCompileCache,
                                                           this.ValidatorCompileCache,
                                                           this.SecurityExpressionBuilderFactoryFunc,
                                                           this.FetchService,
                                                           this.CryptService,
                                                           CurrentTargetSystemTypeResolver,
                                                           session,
                                                           currentPrincipalName,
                                                           this.smtpSettings,
                                                           this.rewriteReceiversService);
        }

        protected override void InitLogger() { }
    }

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
            services.RegisterMessageSenderDependencies<CoreSampleSystemServiceEnvironment, ISampleSystemBLLContext>(configuration);
            services.RegisterRewriteReceiversDependencies(configuration);

            // Others
            services.AddSingleton<IDateTimeService>(DateTimeService.Default);
            services.AddSingleton<IUserAuthenticationService, UserAuthenticationService>();
            services.AddSingleton<ISpecificationEvaluator, NhSpecificationEvaluator>();


            return services.AddSingleton<CoreSampleSystemServiceEnvironment>()
                           .AddControllerEnvironment();
        }

        public static IServiceCollection AddDatabaseSettings(this IServiceCollection services, string connectionString) =>
                services
                        .AddSingleton<IDBSessionFactory, SampleSystemNHibSessionFactory>()
                        .AddSingleton<NHibConnectionSettings>()
                        .AddSingleton<IMappingSettings>(AuthorizationMappingSettings.CreateDefaultAudit(string.Empty))
                        .AddSingleton<IMappingSettings>(ConfigurationMappingSettings.CreateDefaultAudit(string.Empty))
                        .AddSingleton<IMappingSettings>(WorkflowMappingSettings.CreateWithoutAudit(string.Empty))
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
                .AddSingleton<IServiceEnvironment<ISampleSystemBLLContext>>(x => x.GetRequiredService<CoreSampleSystemServiceEnvironment>())
                .AddSingleton<IServiceEnvironment<IAuthorizationBLLContext>>(x => x.GetRequiredService<CoreSampleSystemServiceEnvironment>())
                .AddSingleton<IServiceEnvironment<IConfigurationBLLContext>>(x => x.GetRequiredService<CoreSampleSystemServiceEnvironment>())

                .AddSingleton<IWorkflowServiceEnvironment>(x => x.GetRequiredService<CoreSampleSystemServiceEnvironment>());

            return services;
        }

    }
}

using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Framework.Cap.Abstractions;
using Framework.Configuration.BLL;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.SecuritySystem.Rules.Builders;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;
using Framework.Notification.DTO;
using Framework.NotificationCore.Services;
using Framework.NotificationCore.Settings;
using Framework.Security.Cryptography;
using Framework.Validation;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using nuSpec.Abstraction;
using nuSpec.NHibernate;

using SampleSystem.BLL;
using SampleSystem.Domain;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment.IntegrationTests;
using SampleSystem.IntegrationTests.__Support.TestData.Helpers;
using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment
{
    /// <summary>
    /// TestServiceEnvironment Extends EDServiceEnvironment for Test Run. Different Test Env settings are initilized.
    /// </summary>
    public class TestServiceEnvironment : SampleSystemServiceEnvironment
    {
        private static readonly Lazy<TestServiceEnvironment> IntegrationEnvironmentLazy = new(() => CreateIntegrationEnvironment());

        private static readonly Lazy<TestServiceEnvironment> WorkflowIntegrationEnvironmentLazy = new(() => CreateWorkflowIntegrationEnvironment());

        public TestServiceEnvironment(
            IServiceProvider serviceProvider,
            IDBSessionFactory sessionFactory,
            EnvironmentSettings settings,
            IUserAuthenticationService userAuthenticationService,
            bool? isDebugMode = null)

            : base(serviceProvider, sessionFactory, settings.NotificationContext, userAuthenticationService,
                   new OptionsWrapper<SmtpSettings>(new SmtpSettings() { OutputFolder = @"C:\SampleSystem\Smtp" }),
                  LazyInterfaceImplementHelper.CreateNotImplemented<IRewriteReceiversService>(),
                   isDebugMode)
        {
            this.Settings = settings;
        }

        /// <summary>
        /// Initilize Integration Environment
        /// </summary>
        public static TestServiceEnvironment IntegrationEnvironment => IntegrationEnvironmentLazy.Value;

        public static TestServiceEnvironment WorkflowIntegrationEnvironment => WorkflowIntegrationEnvironmentLazy.Value;

        public bool IsDebugInTest { get; set; } = true;

        /// <summary>
        /// Set IsDebugMode always true for Test run
        /// </summary>
        public override bool IsDebugMode => this.IsDebugInTest;

        /// <summary>
        /// Environment Settings
        /// </summary>
        public EnvironmentSettings Settings { get; }

        private static TestServiceEnvironment CreateIntegrationEnvironment(Action<IServiceCollection> initServices = null)
        {

            return BuildServiceProvider(initServices).GetRequiredService<TestServiceEnvironment>();
        }


        private static IServiceProvider BuildServiceProvider(Action<IServiceCollection> initServices = null)
        {
            return new ServiceCollection()
                                  .RegisterLegacyBLLContext()
                                  .RegisterControllers()
                                  .AddControllerEnvironment()
                                  .AddMediatR(Assembly.GetAssembly(typeof(EmployeeBLL)))
                                  .AddSingleton<SampleSystemServiceEnvironment>(sp => sp.GetRequiredService<TestServiceEnvironment>())
                                  .AddSingleton<IUserAuthenticationService>(IntegrationTestsUserAuthenticationService.Instance)
                                  .AddSingleton<IDateTimeService, IntegrationTestDateTimeService>()
                                  .AddDatabaseSettings(InitializeAndCleanup.DatabaseUtil.ConnectionSettings.ConnectionString)
                                  .AddSingleton(EnvironmentSettings.Trace)
                                  .AddSingleton<TestServiceEnvironment>()
                                  .AddScoped<IExceptionProcessor, ApiControllerExceptionService<TestServiceEnvironment, ISampleSystemBLLContext>>()
                                  .AddSingleton<ISpecificationEvaluator, NhSpecificationEvaluator>()
                                  .AddSingleton<ICapTransactionManager, TestCapTransactionManager>()
                                  .AddSingleton<IIntegrationEventBus, TestIntegrationEventBus>()

                                  .Self(initServices ?? (_ => { }))

                                  .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
        }

        private static TestServiceEnvironment CreateWorkflowIntegrationEnvironment()
        {
            var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", false, true)
                                .AddEnvironmentVariables(nameof(SampleSystem) + "_")
                                .Build();

            var serviceProvider = BuildServiceProvider(services => services.AddWorkflowCore(configuration).AddAuthWorkflow());

            serviceProvider.StartWorkflow();

            return serviceProvider.GetRequiredService<TestServiceEnvironment>();
        }

        protected override SampleSystemBllContextContainer CreateBLLContextContainer(IServiceProvider scopedServiceProvider, IDBSession session, string currentPrincipalName = null)
        {
            return new TestSampleSystemBLLContextContainerStandard(
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

        private class TestSampleSystemBLLContextContainerStandard : SampleSystemBllContextContainer
        {

            public TestSampleSystemBLLContextContainerStandard(SampleSystemServiceEnvironment serviceEnvironment, IServiceProvider scopedServiceProvider, ValidatorCompileCache defaultAuthorizationValidatorCompileCache, ValidatorCompileCache validatorCompileCache, Func<ISampleSystemBLLContext, ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid>> securityExpressionBuilderFactoryFunc, IFetchService<PersistentDomainObjectBase, FetchBuildRule> fetchService, ICryptService<CryptSystem> cryptService, ITypeResolver<string> currentTargetSystemTypeResolver, IDBSession session, string currentPrincipalName, SmtpSettings smtpSettings, IRewriteReceiversService rewriteReceiversService)
                : base(serviceEnvironment, scopedServiceProvider, defaultAuthorizationValidatorCompileCache, validatorCompileCache, securityExpressionBuilderFactoryFunc, fetchService, cryptService, currentTargetSystemTypeResolver, session, currentPrincipalName, smtpSettings, rewriteReceiversService)
            {
            }

            protected override IMessageSender<NotificationEventDTO> GetMessageTemplateSender()
            {
                return new LocalDBNotificationEventDTOMessageSender(this.Configuration);
            }
        }

        private class TestCapTransactionManager : ICapTransactionManager
        {
            public void Enlist(IDbTransaction dbTransaction)
            {
            }
        }

        private class TestIntegrationEventBus : IIntegrationEventBus
        {
            public Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken) => Task.CompletedTask;

            public void Publish(IntegrationEvent @event)
            {
            }
        }
    }
}

using System;
using System.Data;
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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using nuSpec.Abstraction;
using nuSpec.NHibernate;

using AttachmentsSampleSystem.BLL;
using AttachmentsSampleSystem.Domain;
using AttachmentsSampleSystem.IntegrationTests.__Support.ServiceEnvironment.IntegrationTests;
using AttachmentsSampleSystem.IntegrationTests.__Support.TestData.Helpers;
using AttachmentsSampleSystem.ServiceEnvironment;
using AttachmentsSampleSystem.WebApiCore;

namespace AttachmentsSampleSystem.IntegrationTests.__Support.ServiceEnvironment
{
    /// <summary>
    /// TestServiceEnvironment Extends EDServiceEnvironment for Test Run. Different Test Env settings are initilized.
    /// </summary>
    public class TestServiceEnvironment : AttachmentsSampleSystemServiceEnvironment, IControllerEvaluatorContainer
    {
        private static readonly Lazy<TestServiceEnvironment> IntegrationEnvironmentLazy = new Lazy<TestServiceEnvironment>(CreateIntegrationEnvironment);

        public TestServiceEnvironment(
            IServiceProvider serviceProvider,
            IDBSessionFactory sessionFactory,
            EnvironmentSettings settings,
            IUserAuthenticationService userAuthenticationService,
            bool? isDebugMode = null)

            : base(serviceProvider, sessionFactory, settings.NotificationContext, userAuthenticationService, isDebugMode)
        {
            this.Settings = settings;
        }

        /// <summary>
        /// Initilize Integration Environment
        /// </summary>
        public static TestServiceEnvironment IntegrationEnvironment => IntegrationEnvironmentLazy.Value;

        public bool IsDebugInTest { get; set; } = true;

        /// <summary>
        /// Set IsDebugMode always true for Test run
        /// </summary>
        public override bool IsDebugMode => this.IsDebugInTest;

        /// <summary>
        /// Environment Settings
        /// </summary>
        public EnvironmentSettings Settings { get; private set; }
        private static TestServiceEnvironment CreateIntegrationEnvironment()
        {
            var serviceProvider = new ServiceCollection()
                                  .RegisterLegacyBLLContext()
                                  .RegisterControllers()
                                  .AddControllerEnvironment()
                                  .AddMediatR(Assembly.GetAssembly(typeof(EmployeeBLL)))
                                  .AddSingleton<AttachmentsSampleSystemServiceEnvironment>(sp => sp.GetRequiredService<TestServiceEnvironment>())
                                  .AddSingleton<IUserAuthenticationService>(IntegrationTestsUserAuthenticationService.Instance)
                                  .AddSingleton<IDateTimeService>(new IntegrationTestDateTimeService())
                                  .AddDatabaseSettings(InitializeAndCleanup.DatabaseUtil.ConnectionSettings.ConnectionString)
                                  .AddSingleton(EnvironmentSettings.Trace)
                                  .AddSingleton<TestServiceEnvironment>()
                                  .AddScoped<IExceptionProcessor, ApiControllerExceptionService<TestServiceEnvironment, IAttachmentsSampleSystemBLLContext>> ()
                                  .AddSingleton<ISpecificationEvaluator, NhSpecificationEvaluator>()
                                  .AddSingleton<ICapTransactionManager, TestCapTransactionManager>()
                                  .AddSingleton<IIntegrationEventBus, TestIntegrationEventBus>()
                                  .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

            return serviceProvider.GetRequiredService<TestServiceEnvironment>();
        }

        protected override AttachmentsSampleSystemBLLContextContainer CreateBLLContextContainer(IServiceProvider scopedServiceProvider, IDBSession session, string currentPrincipalName = null)
        {
            return new TestAttachmentsSampleSystemBLLContextContainer(
                this,
                scopedServiceProvider,
                this.ValidatorCompileCache,
                this.SecurityExpressionBuilderFactoryFunc,
                this.FetchService,
                this.CryptService,
                CurrentTargetSystemTypeResolver,
                session,
                currentPrincipalName);
        }

        private class TestAttachmentsSampleSystemBLLContextContainer : AttachmentsSampleSystemBLLContextContainer
        {

            public TestAttachmentsSampleSystemBLLContextContainer(AttachmentsSampleSystemServiceEnvironment serviceEnvironment, IServiceProvider scopedServiceProvider, ValidatorCompileCache validatorCompileCache, Func<IAttachmentsSampleSystemBLLContext, ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid>> securityExpressionBuilderFactoryFunc, IFetchService<PersistentDomainObjectBase, FetchBuildRule> fetchService, ICryptService<CryptSystem> cryptService, ITypeResolver<string> currentTargetSystemTypeResolver, IDBSession session, string currentPrincipalName)
                : base(serviceEnvironment, scopedServiceProvider, validatorCompileCache, securityExpressionBuilderFactoryFunc, fetchService, cryptService, currentTargetSystemTypeResolver, session, currentPrincipalName)
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

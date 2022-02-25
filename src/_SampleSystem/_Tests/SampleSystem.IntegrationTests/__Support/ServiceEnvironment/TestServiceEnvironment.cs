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
    public class TestServiceEnvironment : CoreSampleSystemServiceEnvironment
    {
        private static readonly Lazy<TestServiceEnvironment> IntegrationEnvironmentLazy = new Lazy<TestServiceEnvironment>(CreateIntegrationEnvironment);

        public TestServiceEnvironment(
            IServiceProvider serviceProvider,
            IDBSessionFactory sessionFactory,
            EnvironmentSettings settings,
            bool? isDebugMode = null)

            : base(serviceProvider, sessionFactory, settings.NotificationContext, IntegrationTestAuthenticationService.Instance,
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

        public bool IsDebugInTest { get; set; } = true;

        /// <summary>
        /// Set IsDebugMode always true for Test run
        /// </summary>
        public override bool IsDebugMode => this.IsDebugInTest;

        /// <summary>
        /// Environment Settings
        /// </summary>
        public EnvironmentSettings Settings { get; private set; }

        /// <summary>
        /// Нужен для тестов
        /// </summary>
        /// <param name="testValue">Тестовое сохраняемое значение</param>
        public void SaveRegularJobTestValue(string testValue)
        {
            this.GetContextEvaluator().Evaluate(DBSessionMode.Write, context => context.Logics.RegularJobResult.SaveTestValue(testValue));
        }

        private static TestServiceEnvironment CreateIntegrationEnvironment()
        {
            var serviceProvider = new ServiceCollection()
                                  .RegisterLegacyBLLContext()
                                  .RegisterControllers()
                                  .AddControllerEnvironment()
                                  .AddMediatR(Assembly.GetAssembly(typeof(EmployeeBLL)))
                                  .AddSingleton<CoreSampleSystemServiceEnvironment>(sp => sp.GetRequiredService<TestServiceEnvironment>())
                                  .AddSingleton<IUserAuthenticationService>(IntegrationTestAuthenticationService.Instance)
                                  .AddSingleton<IDateTimeService>(new IntegrationTestDateTimeService())
                                  .AddDatabaseSettings(InitializeAndCleanup.DatabaseUtil.ConnectionSettings.ConnectionString)
                                  .AddSingleton(EnvironmentSettings.Trace)
                                  .AddSingleton<TestServiceEnvironment>()
                                  .AddScoped<IExceptionProcessor, ApiControllerExceptionService<TestServiceEnvironment, ISampleSystemBLLContext>> ()
                                  .AddSingleton<ISpecificationEvaluator, NhSpecificationEvaluator>()
                                  .AddSingleton<ICapTransactionManager, TestCapTransactionManager>()
                                  .AddSingleton<IIntegrationEventBus, TestIntegrationEventBus>()
                                  .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });

            return serviceProvider.GetRequiredService<TestServiceEnvironment>();
        }

        protected override SampleSystemBLLContextContainerStandard CreateBLLContextContainer(IServiceProvider scopedServiceProvider, IDBSession session, string currentPrincipalName = null)
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

        private class TestSampleSystemBLLContextContainerStandard : CoreSampleSystemBLLContextContainer
        {

            public TestSampleSystemBLLContextContainerStandard(SampleSystemServiceEnvironmentStandard serviceEnvironment, IServiceProvider scopedServiceProvider, ValidatorCompileCache defaultAuthorizationValidatorCompileCache, ValidatorCompileCache validatorCompileCache, Func<ISampleSystemBLLContext, ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid>> securityExpressionBuilderFactoryFunc, IFetchService<PersistentDomainObjectBase, FetchBuildRule> fetchService, ICryptService<CryptSystem> cryptService, ITypeResolver<string> currentTargetSystemTypeResolver, IDBSession session, string currentPrincipalName, SmtpSettings smtpSettings, IRewriteReceiversService rewriteReceiversService)
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

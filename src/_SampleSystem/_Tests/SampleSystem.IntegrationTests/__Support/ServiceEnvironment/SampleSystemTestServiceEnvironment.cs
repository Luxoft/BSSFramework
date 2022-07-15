using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Framework.Authorization.ApproveWorkflow;
using Framework.Authorization.BLL;
using Framework.Cap.Abstractions;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.Audit;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.NHibernate;
using Framework.DomainDriven.NHibernate.Audit;
using Framework.DomainDriven.ServiceModel;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.SecuritySystem.Rules.Builders;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;
using Framework.NotificationCore.Services;
using Framework.NotificationCore.Settings;
using Framework.Persistent;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using nuSpec.Abstraction;
using nuSpec.NHibernate;

using SampleSystem.BLL;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment.IntegrationTests;
using SampleSystem.IntegrationTests.__Support.TestData.Helpers;
using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment
{
    /// <summary>
    /// SampleSystemTestServiceEnvironment Extends SampleSystemServiceEnvironment for Test Run. Different Test Env settings are initilized.
    /// </summary>
    public class SampleSystemTestServiceEnvironment : SampleSystemServiceEnvironment, IRootServiceProviderContainer
    {
        private static readonly Lazy<SampleSystemTestServiceEnvironment> DefaultLazy = new(CreateDefault);

        public SampleSystemTestServiceEnvironment(
            IServiceProvider serviceProvider,
            AvailableValues availableValues,
            EnvironmentSettings settings,
            bool? isDebugMode = null)

            : base(serviceProvider,
                   new OptionsWrapper<SmtpSettings>(new SmtpSettings() { OutputFolder = @"C:\SampleSystem\Smtp" }),
                    LazyInterfaceImplementHelper.CreateNotImplemented<IRewriteReceiversService>(),
                   isDebugMode)
        {
            this.Settings = settings;
        }

        /// <summary>
        /// Initilize Integration Environment
        /// </summary>
        public static SampleSystemTestServiceEnvironment Default => DefaultLazy.Value;

        public bool IsDebugInTest { get; set; } = true;

        /// <summary>
        /// Set IsDebugMode always true for Test run
        /// </summary>
        public override bool IsDebugMode => this.IsDebugInTest;

        /// <summary>
        /// Environment Settings
        /// </summary>
        public EnvironmentSettings Settings { get; }

        public static SampleSystemTestServiceEnvironment CreateDefault()
        {
            var serviceProvider = BuildServiceProvider();

            serviceProvider.RegisterAuthWorkflow();

            return serviceProvider.GetRequiredService<SampleSystemTestServiceEnvironment>();
        }


        protected static IServiceProvider BuildServiceProvider()
        {
            var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", false, true)
                                .AddEnvironmentVariables(nameof(SampleSystem) + "_")
                                .Build();


            return new ServiceCollection()
                                  .RegisterLegacyBLLContext()
                                  .RegisterControllers()
                                  .AddControllerEnvironment()
                                  .AddMediatR(Assembly.GetAssembly(typeof(EmployeeBLL)))

                                  .AddScoped<IntegrationTestsUserAuthenticationService>()
                                  .AddScoped<IUserAuthenticationService>(sp => sp.GetRequiredService<IntegrationTestsUserAuthenticationService>())

                                  .AddSingleton(EnvironmentSettings.Trace)
                                  .AddSingleton(sp => sp.GetRequiredService<EnvironmentSettings>().NotificationContext)
                                  .AddSingleton(new SubscriptionMetadataStore(new SampleSystemSubscriptionsMetadataFinder()))

                                  .AddSingleton<IAuditRevisionUserAuthenticationService, IntegrationTestsAuditRevisionUserAuthenticationService>()
                                  .AddSingleton<IDateTimeService, IntegrationTestDateTimeService>()
                                  .AddDatabaseSettings(InitializeAndCleanup.DatabaseUtil.ConnectionSettings.ConnectionString)
                                  .AddScoped<IExceptionProcessor, ApiControllerExceptionService<ISampleSystemBLLContext>>()
                                  .AddSingleton<ISpecificationEvaluator, NhSpecificationEvaluator>()
                                  .AddSingleton<ICapTransactionManager, TestCapTransactionManager>()
                                  .AddSingleton<IIntegrationEventBus, TestIntegrationEventBus>()

                                  .AddSingleton<IServiceEnvironment>(sp => sp.GetRequiredService<SampleSystemServiceEnvironment>())
                                  .AddSingleton<SampleSystemServiceEnvironment>(sp => sp.GetRequiredService<SampleSystemTestServiceEnvironment>())
                                  .AddSingleton<SampleSystemTestServiceEnvironment>()

                                  .AddSingleton<IWorkflowManager, WorkflowManager>(sp => sp.GetRequiredService<WorkflowManager>())
                                  .AddSingleton<WorkflowManager>()

                                  .AddScoped<IWorkflowApproveProcessor, WorkflowApproveProcessor>()
                                  .AddScoped<StartWorkflowJob>()
                                  .AddSingleton<SampleSystemTestServiceEnvironment>()
                                  .AddWorkflowCore(configuration)
                                  .AddAuthWorkflow()

                                  .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true, ValidateScopes = true });
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

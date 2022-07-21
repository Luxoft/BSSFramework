using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Framework.Authorization.ApproveWorkflow;
using Framework.Cap.Abstractions;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate.Audit;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;
using Framework.NotificationCore.Services;
using Framework.NotificationCore.Settings;

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
    public static class SampleSystemTestRootServiceProvider
    {
        public static readonly IServiceProvider Default = CreateDefault();

        private static IServiceProvider CreateDefault()
        {
            var serviceProvider = BuildServiceProvider();

            serviceProvider.RegisterAuthWorkflow();

            return serviceProvider;
        }


        private static IServiceProvider BuildServiceProvider()
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
                                  .AddScoped<IImpersonateService>(sp => sp.GetRequiredService<IntegrationTestsUserAuthenticationService>())

                                  .AddSingleton(new SubscriptionMetadataStore(new SampleSystemSubscriptionsMetadataFinder()))

                                  .AddSingleton<IAuditRevisionUserAuthenticationService, IntegrationTestsAuditRevisionUserAuthenticationService>()
                                  .AddSingleton<IDateTimeService, IntegrationTestDateTimeService>()
                                  .AddDatabaseSettings(InitializeAndCleanup.DatabaseUtil.ConnectionSettings.ConnectionString)
                                  .AddScoped<IExceptionProcessor, ApiControllerExceptionService<ISampleSystemBLLContext>>()
                                  .AddSingleton<ISpecificationEvaluator, NhSpecificationEvaluator>()
                                  .AddSingleton<ICapTransactionManager, TestCapTransactionManager>()
                                  .AddSingleton<IIntegrationEventBus, TestIntegrationEventBus>()

                                  .AddSingleton<IWorkflowManager, WorkflowManager>(sp => sp.GetRequiredService<WorkflowManager>())
                                  .AddSingleton<WorkflowManager>()

                                  .AddScoped<IWorkflowApproveProcessor, WorkflowApproveProcessor>()
                                  .AddScoped<StartWorkflowJob>()
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

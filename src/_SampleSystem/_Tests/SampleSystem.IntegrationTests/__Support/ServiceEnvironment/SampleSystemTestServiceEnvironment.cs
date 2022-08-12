using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Framework.Authorization.ApproveWorkflow;

using Framework.Cap.Abstractions;
using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate.Audit;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.WebApiNetCore;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment.IntegrationTests;
using SampleSystem.IntegrationTests.__Support.TestData.Helpers;
using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore;
using SampleSystem.WebApiCore.Env;

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
                                .AddInMemoryCollection(new Dictionary<string, string>
                                 {
                                         {
                                                 "ConnectionStrings:DefaultConnection",
                                                 InitializeAndCleanup.DatabaseUtil.ConnectionSettings.ConnectionString
                                         },
                                 })

                                .Build();


            return new ServiceCollection()

                                  .RegisterLegacyBLLContext()
                                  .AddEnvironment(configuration)
                                  .AddControllerEnvironment()

                                  .AddMediatR(Assembly.GetAssembly(typeof(EmployeeBLL)))

                                  .AddScoped<StartWorkflowJob>()
                                  .AddWorkflowCore(configuration)
                                  .AddAuthWorkflow()

                                  .RegisterControllers()

                                  .AddScoped<IntegrationTestsWebApiCurrentMethodResolver>()
                                  .ReplaceScopedFrom<IWebApiCurrentMethodResolver, IntegrationTestsWebApiCurrentMethodResolver>()

                                  .ReplaceSingleton<IWebApiExceptionExpander, WebApiDebugExceptionExpander>()

                                  .AddSingleton<IntegrationTestDefaultUserAuthenticationService>()
                                  .ReplaceSingletonFrom<IDefaultUserAuthenticationService, IntegrationTestDefaultUserAuthenticationService>()
                                  .ReplaceSingletonFrom<IAuditRevisionUserAuthenticationService, IntegrationTestDefaultUserAuthenticationService>()

                                  .ReplaceSingleton<IDateTimeService, IntegrationTestDateTimeService>()

                                  //.AddSingleton<ICapTransactionManager, TestCapTransactionManager>()
                                  //.AddSingleton<IIntegrationEventBus, TestIntegrationEventBus>()


                                  .AddSingleton<SampleSystemInitializer>()

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

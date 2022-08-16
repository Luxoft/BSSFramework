using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Automation.ServiceEnvironment;
using Automation.ServiceEnvironment.Services;
using Automation.Utils;
using Automation.Utils.DatabaseUtils;
using Automation.Utils.DatabaseUtils.Interfaces;
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
using SampleSystem.IntegrationTests.__Support.TestData.Helpers;
using SampleSystem.IntegrationTests.Support.Utils;
using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore;
using SampleSystem.WebApiCore.Env;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment
{
    public class SampleSystemTestRootServiceProvider
    {
        public static IServiceProvider Create()
        {
            var databaseContext = new DatabaseContext(AppSettings.Default["ConnectionStrings"]);
            return Create(databaseContext);
        }

        public static IServiceProvider Create(IDatabaseContext databaseContext)
        {
            var serviceProvider = BuildServiceProvider(databaseContext);

            serviceProvider.RegisterAuthWorkflow();

            return serviceProvider;
        }

        private static IServiceProvider BuildServiceProvider(IDatabaseContext databaseContext)
        {
            var configuration = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", false, true)
                                .AddEnvironmentVariables(nameof(SampleSystem) + "_")
                                .AddInMemoryCollection(new Dictionary<string, string>
                                 {
                                         { "ConnectionStrings:DefaultConnection", databaseContext.MainDatabase.ConnectionString },
                                         { "ConnectionStrings:WorkflowCoreConnectionString", databaseContext.MainDatabase.ConnectionString },
                                 })
                                .Build();

            return new ServiceCollection()
                    .AddEnvironment(configuration)
                    .RegisterLegacyBLLContext()
                    .AddControllerEnvironment()
                    .AddMediatR(Assembly.GetAssembly(typeof(EmployeeBLL)))
                    .AddScoped<StartWorkflowJob>()
                    .AddWorkflowCore(configuration)
                    .AddAuthWorkflow()
                    .RegisterControllers()
                    .AddSingleton<IDatabaseContext>(databaseContext)
                    .AddSingleton<IDatabaseUtil, SampleSystemDatabaseUtil>()
                    .AddSingleton<DataHelper>()
                    .AddSingleton<AuthHelper>()
                    .AddSingleton<TestDateTimeService>()
                    .AddScoped<TestWebApiCurrentMethodResolver>()
                    .ReplaceScopedFrom<IWebApiCurrentMethodResolver, TestWebApiCurrentMethodResolver>()
                    .ReplaceSingleton<IWebApiExceptionExpander, WebApiDebugExceptionExpander>()
                    .AddSingleton<TestUserAuthenticationService>()
                    .ReplaceSingletonFrom<IDefaultUserAuthenticationService, TestUserAuthenticationService>()
                    .ReplaceSingletonFrom<IAuditRevisionUserAuthenticationService, TestUserAuthenticationService>()
                    .ReplaceSingletonFrom<IDateTimeService, TestDateTimeService>()
                    .AddSingleton<ICapTransactionManager, TestCapTransactionManager>()
                    .AddSingleton<IIntegrationEventBus, TestIntegrationEventBus>()
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

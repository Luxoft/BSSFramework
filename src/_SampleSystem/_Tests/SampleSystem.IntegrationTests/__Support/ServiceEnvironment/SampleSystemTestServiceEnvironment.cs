using System;
using System.Collections.Generic;
using System.Reflection;
using Automation.ServiceEnvironment;
using Automation.ServiceEnvironment.Services;
using Automation.Utils;
using Automation.Utils.DatabaseUtils.Interfaces;
using Framework.Authorization.ApproveWorkflow;
using Framework.Cap.Abstractions;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.WebApiNetCore;
using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.IntegrationTests.__Support.TestData.Helpers;
using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore;
using SampleSystem.WebApiCore.Controllers.Integration;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment
{
    public class SampleSystemTestRootServiceProvider
    {
        public static IServiceProvider Create(IConfiguration configurationBase, IDatabaseContext databaseContext, ConfigUtil configUtil)
        {
            var configuration = new ConfigurationBuilder()
                .AddConfiguration(configurationBase)
                .AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "ConnectionStrings:DefaultConnection", databaseContext.Main.ConnectionString },
                        { "ConnectionStrings:WorkflowCoreConnectionString", databaseContext.Main.ConnectionString },
                    }).Build();

            var provider = TestServiceProvider.Build(
                z =>
                    z.AddEnvironment(configuration)
                        .RegisterLegacyBLLContext()
                        .AddControllerEnvironment()
                        .AddMediatR(Assembly.GetAssembly(typeof(EmployeeBLL)))

                        .AddSingleton<SampleSystemInitializer>()

                        .AddSingleton<ICapTransactionManager, IntegrationTestCapTransactionManager>()
                        .AddSingleton<IIntegrationEventBus, IntegrationTestIntegrationEventBus>()

                        .AddWorkflowCore(configuration)
                        .AddAuthWorkflow()
                        .AddScoped<StartWorkflowJob>()

                        .RegisterControllers(new[] { Assembly.GetAssembly(typeof(EmployeeController)) })

                        .AddSingleton(databaseContext)
                        .AddSingleton<DataHelper>()
                        .AddSingleton<AuthHelper>()
                        .AddSingleton(configUtil)
                    );

            provider.RegisterAuthWorkflow();

            return provider;
        }
    }
}

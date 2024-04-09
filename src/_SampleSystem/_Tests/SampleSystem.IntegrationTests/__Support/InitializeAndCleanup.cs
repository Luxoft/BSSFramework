﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Automation;
using Automation.ServiceEnvironment;

using DotNetCore.CAP;
using DotNetCore.CAP.Internal;

using Framework.Configuration.BLL;
using Framework.Core;
using Framework.DependencyInjection;
using Framework.Notification.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SampleSystem.IntegrationTests.__Support.TestData.Helpers;
using SampleSystem.IntegrationTests.Support.Utils;
using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore.Controllers.Main;
using SampleSystem.WebApiCore;
using SampleSystem.IntegrationTests.__Support.Utils;

namespace SampleSystem.IntegrationTests.__Support;

[TestClass]
public class InitializeAndCleanup
{
    public static readonly TestEnvironment TestEnvironment = new TestEnvironmentBuilder()
                                                             .WithDefaultConfiguration($"{nameof(SampleSystem)}_")
                                                             .WithDatabaseGenerator<SampleSystemTestDatabaseGenerator>()
                                                             .WithServiceProviderBuildFunc(GetServices)
                                                             .Build();

    [AssemblyInitialize]
    public static void EnvironmentInitialize(TestContext testContext)
    {
        TestEnvironment.AssemblyInitializeAndCleanup.EnvironmentInitialize();
    }

    [AssemblyCleanup]
    public static void EnvironmentCleanup()
    {
        TestEnvironment.AssemblyInitializeAndCleanup.EnvironmentCleanup();
    }

    private static IServiceCollection GetServices(IConfiguration configuration, IServiceCollection services)
    {
        return services
               .RegisterGeneralDependencyInjection(configuration)

               .ApplyIntegrationTestServices()

               .ReplaceScoped<IMessageSender<NotificationEventDTO>, LocalDBNotificationEventDTOMessageSender>()

               .AddSingleton<ICapSubscribe, CapIntegrationController>()
               .ReplaceSingleton<IConsumerServiceSelector, TestConsumerServiceSelector>()
               //.ReplaceSingleton<IIntegrationEventBus, IntegrationTestIntegrationEventBus>()
               //.ReplaceSingleton<ICapTransactionManager, IntegrationTestCapTransactionManager>()

               .AddSingleton<SampleSystemInitializer>()

               .RegisterControllers([typeof(EmployeeController).Assembly])

               .AddSingleton<DataHelper>()
               .AddSingleton<AuthHelper>();
    }
}

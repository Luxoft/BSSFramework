using Microsoft.VisualStudio.TestTools.UnitTesting;
using Automation;
using Automation.ServiceEnvironment;

using Bss.Platform.Events.Abstractions;

using Framework.Configuration.BLL;
using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven.Jobs;
using Framework.Notification.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.IntegrationTests.__Support.TestData.Helpers;
using SampleSystem.IntegrationTests.Support.Utils;
using SampleSystem.ServiceEnvironment;
using SampleSystem.WebApiCore.Controllers.Main;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.ServiceEnvironment.Jobs;

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
    public static async Task EnvironmentInitialize(TestContext testContext) =>
        await TestEnvironment.AssemblyInitializeAndCleanup.EnvironmentInitializeAsync();

    [AssemblyCleanup]
    public static async Task EnvironmentCleanup() =>
        await TestEnvironment.AssemblyInitializeAndCleanup.EnvironmentCleanupAsync();

    private static IServiceCollection GetServices(IConfiguration configuration, IServiceCollection services)
    {
        return services
               .RegisterGeneralDependencyInjection(configuration)

               .AddSingleton<SampleSystemInitializer>()

               .ApplyIntegrationTestServices()

               .ReplaceScoped<IMessageSender<NotificationEventDTO>, LocalDBNotificationEventDTOMessageSender>()
               .AddScoped<IIntegrationEventPublisher, TestIntegrationEventPublisher>()

               .AddSingleton(new JobImpersonateData("sampleSystemTestJob"))
               .RegisterJobs([typeof(SampleJob).Assembly])

               .RegisterControllers([typeof(EmployeeController).Assembly])

               .AddSingleton<DataHelper>()
               .AddSingleton<AuthHelper>()

               .AddSingleton<TestDataInitializer>();
    }
}

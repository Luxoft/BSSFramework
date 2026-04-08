using Bss.Platform.Events.Abstractions;

using Framework.Application.Jobs;
using Framework.AutomationCore.Environment;
using Framework.AutomationCore.ServiceEnvironment.ServiceEnvironment;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;

using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.IntegrationTests.__Support.TestData.Helpers;
using SampleSystem.IntegrationTests.__Support.Utils;
using SampleSystem.ServiceEnvironment;
using SampleSystem.ServiceEnvironment.DependencyInjection;
using SampleSystem.ServiceEnvironment.Jobs;
using SampleSystem.WebApiCore.Controllers.Main;

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

    private static IServiceCollection GetServices(IConfiguration configuration, IServiceCollection services) =>
        services
            .AddGeneralDependencyInjection(configuration, new HostingEnvironment(), s => s.AddExtensions(new SampleSystemNHibernateExtension()))

            .AddSingleton<SampleSystemInitializer>()

            .AddIntegrationTests()

            .AddScoped<IIntegrationEventPublisher, TestIntegrationEventPublisher>()

            .AddSingleton(new JobImpersonateData("sampleSystemTestJob"))
            .AddJobs([typeof(SampleJob).Assembly])

            .AddTestControllers([typeof(EmployeeController).Assembly])

            .AddSingleton<DataHelper>()

            .AddSingleton<TestDataInitializer>();
}

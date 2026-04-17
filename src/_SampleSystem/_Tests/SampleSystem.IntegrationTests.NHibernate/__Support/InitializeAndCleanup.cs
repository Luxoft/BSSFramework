using Bss.Platform.Events.Abstractions;

using Framework.Application.Jobs;
using Framework.AutomationCore.Environment;
using Framework.AutomationCore.ServiceEnvironment;

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

public class AssemblyFixture : IAsyncLifetime
{
    public static readonly TestEnvironment TestEnvironment = new TestEnvironmentBuilder()
                                                             .WithDefaultConfiguration($"{nameof(SampleSystem)}_")
                                                             .WithDatabaseGenerator<SampleSystemTestDatabaseGenerator>()
                                                             .WithServiceProviderBuildFunc(GetServices)
                                                             .Build();

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

    public Task InitializeAsync() => TestEnvironment.AssemblyInitializeAndCleanup.EnvironmentInitializeAsync();

    public async Task DisposeAsync()
    {
        await TestEnvironment.AssemblyInitializeAndCleanup.EnvironmentCleanupAsync();
        GC.SuppressFinalize(this);
    }
}

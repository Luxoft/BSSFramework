using Anch.Testing.Database.DependencyInjection;
using Anch.Testing.Xunit;

using Bss.Platform.Events.Abstractions;

using Framework.Application.Jobs;
using Framework.AutomationCore;
using Framework.AutomationCore.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.Internal;

using SampleSystem.IntegrationTests._Environment;
using SampleSystem.IntegrationTests._Environment.TestData;
using SampleSystem.IntegrationTests._Environment.TestData.Helpers;
using SampleSystem.IntegrationTests._Environment.Utils;
using SampleSystem.ServiceEnvironment;
using SampleSystem.ServiceEnvironment.DependencyInjection;
using SampleSystem.ServiceEnvironment.Jobs;
using SampleSystem.WebApiCore.Controllers.Main;

[assembly: AnchTestFramework<NHibTestEnvironment>]
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace SampleSystem.IntegrationTests._Environment;

public class NHibTestEnvironment : BssTestEnvironment
{
    private const string SettingsFileName = "testAppSettings.json";

    protected override IConfiguration RawConfiguration { get; } = new ConfigurationBuilder()
                                                                   .SetBasePath(Directory.GetCurrentDirectory())
                                                                   .AddJsonFile(SettingsFileName, false, true)
                                                                   .AddJsonFile($"{Environment.MachineName}.{SettingsFileName}", true)
                                                                   .AddEnvironmentVariables($"{nameof(SampleSystem)}_").Build();

    protected override void SetInitializers(IDatabaseTestingSetup setup) =>

        setup.SetEmptySchemaInitializer<EmptySchemaInitializer>()
             .SetTestDataInitializer<TestDataInitializer>();


    protected override void InitializeServices(IServiceCollection services, IConfiguration configuration) =>

        services.AddGeneralDependencyInjection(configuration, new HostingEnvironment(), s => s.AddExtensions(new SampleSystemNHibernateExtension()))

                .AddSingleton<SampleSystemInitializer>()

                .AddIntegrationTests()

                .AddScoped<IIntegrationEventPublisher, TestIntegrationEventPublisher>()

                .AddSingleton(new JobImpersonateData("sampleSystemTestJob"))
                .AddJobs([typeof(SampleJob).Assembly])

                .AddTestControllers([typeof(EmployeeController).Assembly])

                .AddSingleton<DataManager>();
}

using Anch.DependencyInjection;
using Anch.Testing;
using Anch.Testing.Database;
using Anch.Testing.Database.DependencyInjection;
using Anch.Testing.Database.Mssql;
using Anch.Testing.Xunit;

using Bss.Platform.Events.Abstractions;

using Framework.Application.Jobs;
using Framework.AutomationCore.ServiceEnvironment;
using Framework.AutomationCore.Settings;
using Framework.Database;
using Framework.Database.ConnectionStringSource;

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

namespace SampleSystem.IntegrationTests._Environment;

public class NHibTestEnvironment : ITestEnvironment
{
    private readonly IConfiguration configuration;

    private readonly AutomationFrameworkSettings settings = new();

    private readonly TestDatabaseSettings databaseSettings;

    public NHibTestEnvironment()
    {
        var settingsFileName = "testAppSettings.json";

        this.configuration = new ConfigurationBuilder()
                             .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile(settingsFileName, false, true)
                             .AddJsonFile($"{Environment.MachineName}.{settingsFileName}", true)
                             .AddEnvironmentVariables(nameof(SampleSystem)).Build();

        this.configuration.Bind(this.settings);

        var defaultConnectionString = this.configuration.GetConnectionString("DefaultConnection")
                                      ?? throw new InvalidOperationException("DefaultConnection string is not configured.");

        this.databaseSettings = new TestDatabaseSettings { InitMode = this.settings.DatabaseInitMode, DefaultConnectionString = new(defaultConnectionString) };
    }

    public bool AllowParallelization => this.settings.TestsParallelize;

    public IServiceProvider BuildServiceProvider(IServiceCollection services)
    {
        services.AddOptions<AutomationFrameworkSettings>();

        return services.AddSingleton(this.configuration)

                       .AddGeneralDependencyInjection(this.configuration, new HostingEnvironment(), s => s.AddExtensions(new SampleSystemNHibernateExtension()))

                       .AddSingleton<SampleSystemInitializer>()

                       .AddIntegrationTests()

                       .AddScoped<IIntegrationEventPublisher, TestIntegrationEventPublisher>()

                       .AddSingleton(new JobImpersonateData("sampleSystemTestJob"))
                       .AddJobs([typeof(SampleJob).Assembly])

                       .AddTestControllers([typeof(EmployeeController).Assembly])

                       .AddSingleton<DataManager>()

                       .AddSingleton<EmptySchemaInitializer>()
                       .AddSingleton<TestDataInitializer>()

                       .AddDatabaseTesting(dts => dts
                                                  .SetProvider<MssqlDatabaseTestingProvider>()
                                                  .SetEmptySchemaInitializer<EmptySchemaInitializer>()
                                                  .SetTestDataInitializer<TestDataInitializer>()
                                                  .SetSettings(this.databaseSettings)
                                                  .RebindActualConnection<IDefaultConnectionStringSource>(connectionString =>
                                                          new ManualDefaultConnectionStringSource(connectionString.Value)))

                       .AddValidator<DuplicateServiceUsageValidator>()
                       .BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });
    }
}

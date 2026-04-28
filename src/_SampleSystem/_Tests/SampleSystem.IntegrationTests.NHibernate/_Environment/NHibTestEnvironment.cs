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
using Framework.AutomationCore.Utils.DatabaseUtils;
using Framework.AutomationCore.Utils.DatabaseUtils.Interfaces;
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
    public IServiceProvider BuildServiceProvider(IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("testAppSettings.json", false, true).Build();

        var automationFrameworkSettings = new AutomationFrameworkSettings();
        configuration.Bind(nameof(AutomationFrameworkSettings), automationFrameworkSettings);

        var defaultConnectionString = configuration.GetConnectionString("DefaultConnection")
                                      ?? throw new InvalidOperationException("DefaultConnection string is not configured.");

        return services.AddSingleton<IConfiguration>(configuration)

                       .AddGeneralDependencyInjection(configuration, new HostingEnvironment(), s => s.AddExtensions(new SampleSystemNHibernateExtension()))

                       .AddSingleton<SampleSystemInitializer>()

                       .AddIntegrationTests()

                       .AddScoped<IIntegrationEventPublisher, TestIntegrationEventPublisher>()

                       .AddSingleton(new JobImpersonateData("sampleSystemTestJob"))
                       .AddJobs([typeof(SampleJob).Assembly])

                       .AddTestControllers([typeof(EmployeeController).Assembly])

                       .AddSingleton<DataHelper>()

                       .AddSingleton<EmptySchemaInitializer>()
                       .AddSingleton<TestDataInitializer>()

                       .AddDatabaseTesting(dts => dts
                                                  .SetProvider<MssqlDatabaseTestingProvider>()
                                                  .SetEmptySchemaInitializer<EmptySchemaInitializer>()
                                                  .SetSharedTestDataInitializer<TestDataInitializer>()
                                                  .SetSettings(
                                                      new TestDatabaseSettings
                                                      {
                                                          InitMode = automationFrameworkSettings.DatabaseInitMode,
                                                          DefaultConnectionString = new(defaultConnectionString)
                                                      })
                                                  .RebindActualConnection<IDefaultConnectionStringSource>(connectionString =>
                                                      new ManualDefaultConnectionStringSource(connectionString.Value)))

                       .AddValidator<DuplicateServiceUsageValidator>()
                       .BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });
    }
}

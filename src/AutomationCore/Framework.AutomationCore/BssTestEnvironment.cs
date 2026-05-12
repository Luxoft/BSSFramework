using Anch.Core;
using Anch.DependencyInjection;
using Anch.Testing;
using Anch.Testing.Database;
using Anch.Testing.Database.ConnectionStringManagement;
using Anch.Testing.Database.DependencyInjection;

using Framework.AutomationCore.ServiceEnvironment;
using Framework.AutomationCore.Settings;
using Framework.AutomationCore.TestingProvider;
using Framework.Core;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.AutomationCore;

public abstract class BssTestEnvironment : ITestEnvironment
{
    protected abstract IConfiguration GetRootConfiguration();

    private IConfiguration RootConfiguration => field ??= this.GetRootConfiguration();

    private AutomationFrameworkSettings Settings =>
        field ??= new AutomationFrameworkSettings().Self(this.RootConfiguration.GetSection(nameof(AutomationFrameworkSettings)).Bind);

    protected virtual string DefaultConnectionStringName => "DefaultConnection";

    private string DefaultConnectionString => field ??= this.RootConfiguration.GetConnectionString(this.DefaultConnectionStringName)
                                                        ?? throw new InvalidOperationException(
                                                            $"{this.DefaultConnectionStringName} connection string is not configured.");

    private TestDatabaseSettings TestDatabaseSettings =>
        field ??= new TestDatabaseSettings { InitMode = this.Settings.DatabaseInitMode, DefaultConnectionString = new(this.DefaultConnectionString) };

    public bool AllowParallelization => this.Settings.TestsParallelize;

    protected virtual void InitServices(IServiceCollection services, IConfiguration configuration)
    {

    }

    protected virtual void InitInitializers(IDatabaseTestingSetup setup)
    {

    }

    public IServiceProvider BuildServiceProvider(IServiceCollection services)
    {
        var actualConnectionString = this.GetActualConnectionString(services);

        var actualConfiguration = this.GetActualConfiguration(actualConnectionString);

        services.AddOptions<AutomationFrameworkSettings>().Bind(actualConfiguration.GetSection(nameof(AutomationFrameworkSettings)));

        return services.Self(v => this.InitServices(v, actualConfiguration))
                       .AddIntegrationTests()
                       .AddDatabaseTesting(dts => dts

                                                  .SetProvider<BssDatabaseTestingProvider>()
                                                  .Self(this.InitInitializers)
                                                  .SetSettings(this.TestDatabaseSettings)
                                                  .RebindActualConnection(newActualConnectionString =>
                                                                              newActualConnectionString == actualConnectionString
                                                                                  ? actualConfiguration
                                                                                  : throw new InvalidOperationException(
                                                                                        "Actual connection string does not match the expected connection string.")))
                       .Pipe(this.InternalBuildServiceProvider);
    }

    private TestDatabaseConnectionString GetActualConnectionString(IServiceCollection services)
    {
        var serviceProvider = services.AddSingleton(this.TestDatabaseSettings)
                                      .AddSingleton<ITestConnectionStringProvider, TestConnectionStringProvider>()
                                      .AddSingleton<ITestDatabaseConnectionStringBuilder, BssTestDatabaseConnectionStringBuilder>()
                                      .BuildDefaultServiceProvider();

        return serviceProvider.GetRequiredService<ITestConnectionStringProvider>().Actual;
    }

    protected virtual IConfiguration GetActualConfiguration(TestDatabaseConnectionString connectionString) =>

        new ConfigurationBuilder()
            .AddConfiguration(this.RootConfiguration)
            .AddInMemoryCollection(new Dictionary<string, string?> { [$"ConnectionStrings:{this.DefaultConnectionStringName}"] = connectionString.Value })
            .Build();

    protected virtual IServiceProvider InternalBuildServiceProvider(IServiceCollection services) => services.BuildDefaultServiceProvider();
}

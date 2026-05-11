using Anch.Core;
using Anch.DependencyInjection;
using Anch.Testing;
using Anch.Testing.Database;
using Anch.Testing.Database.DependencyInjection;

using Framework.AutomationCore.ServiceEnvironment;
using Framework.AutomationCore.Settings;
using Framework.AutomationCore.TestingProvider;
using Framework.Core;
using Framework.Database;
using Framework.Database.ConnectionStringSource;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.AutomationCore;

public abstract class BssTestEnvironment : ITestEnvironment
{
    protected abstract IConfiguration Configuration { get; }

    private AutomationFrameworkSettings Settings => field ??= new AutomationFrameworkSettings().Self(this.Configuration.Bind);

    protected virtual string DefaultConnectionStringName => "DefaultConnection";

    protected virtual string DefaultConnectionString => field ??= this.Configuration.GetConnectionString(this.DefaultConnectionStringName)
                                                                  ?? throw new InvalidOperationException(
                                                                      $"{this.DefaultConnectionStringName} connection string is not configured.");

    private TestDatabaseSettings TestDatabaseSettings =>
        field ??= new TestDatabaseSettings { InitMode = this.Settings.DatabaseInitMode, DefaultConnectionString = new(this.DefaultConnectionString) };

    public bool AllowParallelization => this.Settings.TestsParallelize;

    protected virtual void InitServices(IServiceCollection services)
    {

    }

    protected virtual void InitInitializers(IDatabaseTestingSetup setup)
    {

    }

    public IServiceProvider BuildServiceProvider(IServiceCollection services)
    {
        services.AddOptions<AutomationFrameworkSettings>();

        return services.AddSingleton(this.Configuration)

                       .Self(this.InitServices)

                       .AddIntegrationTests()

                       .AddDatabaseTesting(dts => dts
                                                  .SetProvider<BssDatabaseTestingProvider>()
                                                  .Self(this.InitInitializers)
                                                  .SetSettings(this.TestDatabaseSettings)
                                                  .RebindActualConnection<IDefaultConnectionStringSource>(connectionString =>
                                                      new ManualDefaultConnectionStringSource(connectionString.Value)))

                       .Pipe(this.InternalBuildServiceProvider);
    }

    protected virtual IServiceProvider InternalBuildServiceProvider(IServiceCollection services) =>
        services.AddValidator<DuplicateServiceUsageValidator>()
                .BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });
}

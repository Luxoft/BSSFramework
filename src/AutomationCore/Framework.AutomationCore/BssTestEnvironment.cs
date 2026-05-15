using Anch.Core;
using Anch.Testing;
using Anch.Testing.Database;
using Anch.Testing.Database.Configuration;
using Anch.Testing.Database.ConnectionStringManagement;
using Anch.Testing.Database.DependencyInjection;
using Framework.AutomationCore.ServiceEnvironment;
using Framework.AutomationCore.TestingProvider;
using Framework.Core;
using Framework.Database.ConnectionStringSource;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.AutomationCore;

public abstract class BssTestEnvironment : ConfigurationTestEnvironment
{
    private AutomationFrameworkSettings Settings =>
        field ??= new AutomationFrameworkSettings().Self(this.RawConfiguration.GetSection(nameof(AutomationFrameworkSettings)).Bind);

    protected override string ConnectionStringName { get; } = DefaultConnectionStringSettings.Default.Name;

    protected override DatabaseInitMode DatabaseInitMode => this.Settings.DatabaseInitMode;

    protected virtual void InitializeServices(IServiceCollection services, IConfiguration configuration)
    {
    }

    protected virtual void SetInitializers(IDatabaseTestingSetup setup)
    {
    }

    protected sealed override void InitDatabase(IDatabaseTestingSetup dts) =>
        dts.SetProvider<BssDatabaseTestingProvider>()
           .SetTestDataInitializer<BssDatabaseSnapshotInitializer>()
           .Self(this.SetInitializers)
           .SetParallelization(this.Settings.TestsParallelize);

    protected override IServiceProvider BuildServiceProvider(IServiceCollection services, IConfiguration actualConfiguration)
    {
        services.AddOptions<AutomationFrameworkSettings>().Bind(actualConfiguration.GetSection(nameof(AutomationFrameworkSettings)));

        return services.Self(v => this.InitializeServices(v, actualConfiguration))
                       .AddIntegrationTests()
                       .Pipe(this.InternalBuildServiceProvider);
    }

    protected virtual IServiceProvider InternalBuildServiceProvider(IServiceCollection services) => services.BuildDefaultServiceProvider();
}

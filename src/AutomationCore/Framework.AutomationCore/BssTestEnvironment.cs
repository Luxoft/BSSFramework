using Anch.Core;
using Anch.DependencyInjection;
using Anch.Testing;
using Anch.Testing.Database;
using Anch.Testing.Database.Configuration;
using Anch.Testing.Database.DependencyInjection;

using Framework.AutomationCore.Extensions;
using Framework.AutomationCore.Services;
using Framework.Core;
using Framework.Database.ConnectionStringSource;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.AutomationCore;

public abstract class BssTestEnvironment : ConfigurationTestEnvironment
{
    private AutomationFrameworkSettings RawSettings =>
        field ??= new AutomationFrameworkSettings().Self(this.RawConfiguration.GetSection(nameof(AutomationFrameworkSettings)).Bind)
                                                   .Self(this.SetupSettings);

    protected override string ConnectionStringName { get; } = DefaultConnectionStringSettings.Default.Name;

    protected sealed override DatabaseInitMode DatabaseInitMode => this.RawSettings.DatabaseInitMode;

    protected abstract void InitializeServices(IServiceCollection services, IConfiguration configuration);

    protected abstract void SetInitializers(IDatabaseTestingSetup setup);

    protected virtual void SetupSettings(AutomationFrameworkSettings settings)
    {
    }

    protected sealed override void InitDatabase(IDatabaseTestingSetup dts) =>
        dts.SetProvider<BssDatabaseTestingProvider>()
           .SetDatabaseSnapshotInitializer<BssDatabaseSnapshotInitializer>()
           .Self(this.SetInitializers)
           .SetParallelization(this.RawSettings.TestsParallelize);

    protected override IServiceProvider BuildServiceProvider(IServiceCollection services, IConfiguration actualConfiguration)
    {
        services.AddOptions<AutomationFrameworkSettings>().Bind(actualConfiguration.GetSection(nameof(AutomationFrameworkSettings)));

        return services
               .AddEnvironmentHook<BssCleanupTestEnvironmentHook>(EnvironmentHookType.After)
               .Self(v => this.InitializeServices(v, actualConfiguration))
               .AddIntegrationTests(this.SetupSettings)
               .Pipe(v => this.InternalBuildServiceProvider(v, actualConfiguration));
    }

    protected virtual IServiceProvider InternalBuildServiceProvider(IServiceCollection services, IConfiguration actualConfiguration) =>
        services.AddValidator<DuplicateServiceUsageValidator>()
                .Validate()
                .BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true, ValidateOnBuild = true });
}


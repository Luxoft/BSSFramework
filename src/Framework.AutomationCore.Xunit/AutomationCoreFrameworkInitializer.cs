using Automation.Extensions;
using Automation.Settings;
using Automation.Utils.DatabaseUtils;
using Automation.Utils.DatabaseUtils.Interfaces;
using Automation.Xunit.ServiceProviderPool;

using Bss.Testing.Xunit.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Automation.Xunit;

public abstract class AutomationCoreFrameworkInitializer : IFrameworkInitializer
{
    public abstract IServiceCollection ConfigureFramework(IServiceCollection services);

    public abstract IServiceProvider ConfigureTestEnvironment(IServiceCollection services, IConfiguration configuration);

    public IServiceProvider GetFrameworkServiceProvider()
    {
        var frameworkServices = new ServiceCollection();
        AddSettings(frameworkServices);
        this.ConfigureFramework(frameworkServices);

        frameworkServices.TryAddSingleton<IDatabaseContext, DatabaseContext>();
        frameworkServices.TryAddSingleton<ITestServiceProviderPool, DiServiceProviderPool>();

        frameworkServices.AddSingleton(
            sp => new ServiceProviderPoolFunc(this.GetPoolServiceProviderFunc(sp)));

        return frameworkServices.BuildServiceProvider(
            new ServiceProviderOptions
            {
                ValidateScopes = true,
                ValidateOnBuild = true
            });
    }

    private Func<IServiceProvider> GetPoolServiceProviderFunc(IServiceProvider frameworkSp)
    {
        var rootConfiguration = frameworkSp.GetRequiredService<IConfiguration>();
        var automationFrameworkSettings = frameworkSp.GetRequiredService<IOptions<AutomationFrameworkSettings>>();

        return () =>
               {
                   var databaseContext = new DatabaseContext(rootConfiguration, automationFrameworkSettings);
                   var configuration = rootConfiguration.BuildFromRootWithConnectionStrings(databaseContext);

                   var environmentServices = new ServiceCollection()
                                             .AddSingleton<IDatabaseContext>(databaseContext)
                                             .AddSingleton<IConfiguration>(configuration)
                                             .AddSingleton(automationFrameworkSettings);

                   return this.ConfigureTestEnvironment(environmentServices, configuration);
               };
    }

    private static void AddSettings(IServiceCollection services)
    {
        services.AddOptions<AutomationFrameworkSettings>();
        services.AddSingleton<IConfigureOptions<AutomationFrameworkSettings>>(
            s =>
            {
                return new ConfigureOptions<AutomationFrameworkSettings>(
                    opt => s.GetRequiredService<IConfiguration>()
                            .Bind(nameof(AutomationFrameworkSettings), opt));
            });
    }
}


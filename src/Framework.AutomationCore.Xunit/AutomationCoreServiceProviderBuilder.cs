using System.Reflection;

using Automation.Extensions;
using Automation.Utils;
using Automation.Utils.DatabaseUtils;
using Automation.Utils.DatabaseUtils.Interfaces;
using Automation.Xunit.Interfaces;
using Automation.Xunit.Utils;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Automation.Xunit;

public class AutomationCoreServiceProviderBuilder : IAutomationCoreServiceProviderBuilder
{
    public IServiceProvider GetFrameworkServiceProvider(AssemblyName assemblyName)
    {
        var initializationType = FrameworkInitializationUtil.GetSingleTypeImplementation<IAutomationCoreInitialization>(assemblyName);
        var initializationObject = FrameworkInitializationUtil.GetObjectInstance<IAutomationCoreInitialization>(initializationType);

        var frameworkServices = new ServiceCollection();
        frameworkServices.AddOptions<DatabaseContextSettings>();
        ConfigureFramework(initializationType, initializationObject, frameworkServices).Invoke();

        frameworkServices.TryAddSingleton<ConfigUtil>();
        frameworkServices.TryAddSingleton<IDatabaseContext, DatabaseContext>();
        frameworkServices.TryAddSingleton<IServiceProviderPool, DiServiceProviderPool>();

        frameworkServices.AddSingleton(
            sp => new ServiceProviderPoolFunc(this.GetPoolServiceProviderFunc(initializationType, sp)));

        return frameworkServices.BuildServiceProvider(
            new ServiceProviderOptions
            {
                ValidateScopes = true,
                ValidateOnBuild = true
            });
    }

    protected Func<IServiceProvider> GetPoolServiceProviderFunc(Type initializationType, IServiceProvider frameworkSp)
    {
        var initializationObject = FrameworkInitializationUtil.GetObjectInstance<IAutomationCoreInitialization>(initializationType);
        var configureEnvironmentMethod = FrameworkInitializationUtil.GetSingleMethod(
            initializationType,
            nameof(IAutomationCoreInitialization.ConfigureTestEnvironment));

        var rootConfiguration = frameworkSp.GetRequiredService<IConfiguration>();
        var configUtil = frameworkSp.GetRequiredService<ConfigUtil>();
        var databaseContextSettings = frameworkSp.GetRequiredService<IOptions<DatabaseContextSettings>>();

        return () =>
               {
                   var databaseContext = new DatabaseContext(configUtil, databaseContextSettings);
                   var configuration = rootConfiguration.BuildFromRootWithConnectionStrings(databaseContext);

                   var environmentServices = new ServiceCollection()
                                             .AddSingleton<IDatabaseContext>(databaseContext)
                                             .AddSingleton<IConfiguration>(configuration)
                                             .AddSingleton(configUtil);

                   return (IServiceProvider)configureEnvironmentMethod.Invoke(
                       initializationObject,
                       new object[] { environmentServices, configuration });
               };
    }

    private static Func<IServiceCollection> ConfigureFramework(
        Type initializationType,
        object initializationObject,
        IServiceCollection serviceCollection)
    {
        var configureFrameworkMethod = FrameworkInitializationUtil.GetSingleMethod(
            initializationType,
            nameof(IAutomationCoreInitialization.ConfigureFramework));

        return () => (IServiceCollection)configureFrameworkMethod.Invoke(initializationObject, new object[] { serviceCollection });
    }
}

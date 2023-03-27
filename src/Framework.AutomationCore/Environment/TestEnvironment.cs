using Automation.Utils;
using Automation.Utils.DatabaseUtils;
using Automation.Utils.DatabaseUtils.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Automation;

public class TestEnvironment
{
    public readonly AssemblyInitializeAndCleanup AssemblyInitializeAndCleanup;

    public readonly ServiceProviderPool ServiceProviderPool;

    internal TestEnvironment(
            IConfiguration rootConfiguration,
            Func<IConfiguration, IServiceCollection, IServiceCollection> serviceProviderBuildFunc,
            Action<IServiceProvider> serviceProviderAfterBuildAction,
            Type databaseGenerator,
            string connectionStringName,
            string[] secondaryDatabases)
    {
        this.ServiceProviderPool = new ServiceProviderPool(
                                                           () => this.ServiceProviderGenerationFunc(
                                                            serviceProviderBuildFunc,
                                                            serviceProviderAfterBuildAction,
                                                            rootConfiguration,
                                                            databaseGenerator,
                                                            connectionStringName,
                                                            secondaryDatabases),
                                                           rootConfiguration.GetValue<bool>("TestsParallelize"));

        this.AssemblyInitializeAndCleanup = new AssemblyInitializeAndCleanup(
                                                                             () => this.ServiceProviderPool.Get(),
                                                                             (serviceProvider) =>
                                                                                     this.ServiceProviderPool.Release(
                                                                                      serviceProvider));
    }

    private ServiceProvider ServiceProviderGenerationFunc(
            Func<IConfiguration, IServiceCollection, IServiceCollection> serviceProviderBuildFunc,
            Action<IServiceProvider> serviceProviderAfterBuildAction,
            IConfiguration rootConfiguration,
            Type databaseGenerator,
            string connectionStringName,
            string[] secondaryDatabases)
    {
        var configUtil = new ConfigUtil(rootConfiguration);
        var databaseContextSettings =
                new DatabaseContextSettings(configUtil.GetConnectionString(connectionStringName), secondaryDatabases);
        var databaseContext = new DatabaseContext(configUtil, databaseContextSettings);
        var configuration = new ConfigurationBuilder()
                            .AddConfiguration(rootConfiguration)
                            .AddInMemoryCollection(
                                                   rootConfiguration.GetSection("ConnectionStrings")
                                                                    .GetChildren()
                                                                    .ToDictionary(
                                                                                  x => $"ConnectionStrings:{x.Key}",
                                                                                  _ => databaseContext.Main.ConnectionString))
                            .Build();

        var environmentServices = new ServiceCollection();
        serviceProviderBuildFunc.Invoke(configuration, environmentServices);
        environmentServices.TryAddSingleton(databaseContextSettings);
        environmentServices.TryAddSingleton<IDatabaseContext>(databaseContext);
        environmentServices.AddSingleton<IConfiguration>(configuration)
                           .AddSingleton<ConfigUtil>()
                           .AddSingleton(typeof(TestDatabaseGenerator), databaseGenerator);

        var environmentServiceProvider = environmentServices.BuildServiceProvider(
            new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

        serviceProviderAfterBuildAction?.Invoke(environmentServiceProvider);

        return environmentServiceProvider;
    }
}

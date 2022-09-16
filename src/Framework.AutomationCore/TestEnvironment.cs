using System;
using System.IO;

using Automation.Utils;
using Automation.Utils.DatabaseUtils;
using Automation.Utils.DatabaseUtils.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Automation;

public abstract class TestEnvironment
{
    private readonly Lazy<IConfiguration> lazyRootConfiguration;

    private readonly Lazy<ConfigUtil> lazyConfigUtil;

    private readonly Lazy<AssemblyInitializeAndCleanup> lazyAssemblyInitializeAndCleanup;

    private readonly Lazy<ServiceProviderPool> lazyServiceProviderPool;

    protected TestEnvironment()
    {
        this.lazyRootConfiguration = new Lazy<IConfiguration>(this.BuildConfiguration);

        this.lazyConfigUtil = new Lazy<ConfigUtil>(new ConfigUtil(this.RootConfiguration));

        this.lazyAssemblyInitializeAndCleanup = new Lazy<AssemblyInitializeAndCleanup>(
            () =>
            {
                var serviceProvider = this.ServiceProviderPool.Get();

                var databaseContext = serviceProvider.GetRequiredService<IDatabaseContext>();

                return new AssemblyInitializeAndCleanup(
                    this.ConfigUtil,
                    this.GetDatabaseGenerator(serviceProvider, databaseContext),
                    () => this.ServiceProviderPool.Release(serviceProvider));
            });

        this.lazyServiceProviderPool = new Lazy<ServiceProviderPool>(this.BuildServiceProvidePool);
    }

    public AssemblyInitializeAndCleanup AssemblyInitializeAndCleanup => this.lazyAssemblyInitializeAndCleanup.Value;

    public ConfigUtil ConfigUtil => this.lazyConfigUtil.Value;

    public IConfiguration RootConfiguration => this.lazyRootConfiguration.Value;

    protected abstract string EnvironmentPrefix { get; }

    public ServiceProviderPool ServiceProviderPool => this.lazyServiceProviderPool.Value;

    public virtual IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($@"appsettings.json", false)
            .AddJsonFile($@"{Environment.MachineName}.appsettings.json", true)
            .AddEnvironmentVariables(this.EnvironmentPrefix)
            .Build();
    }

    protected abstract ServiceProviderPool BuildServiceProvidePool();

    protected abstract TestDatabaseGenerator GetDatabaseGenerator(IServiceProvider serviceProvider, IDatabaseContext databaseContext);
}

using Automation.Extensions;
using Automation.Utils;
using Automation.Utils.DatabaseUtils;
using Automation.Utils.DatabaseUtils.Interfaces;

using Framework.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Automation;

public class TestEnvironmentBuilder
{
    private IConfiguration withConfiguration;
    private Func<IConfiguration, IServiceCollection, IServiceCollection> withServiceProviderBuildFunc;
    private Action<IServiceProvider> withServiceProvAfterBuildAction;
    private Type withDatabaseGenerator;
    private string withConnectionStringName;
    private string[] withSecondaryDatabases;

    public TestEnvironmentBuilder WithConfiguration(IConfiguration rootConfiguration)
    {
        this.withConfiguration = rootConfiguration;

        return this;
    }

    public TestEnvironmentBuilder WithDefaultConfiguration(
        string environmentVariablesPrefix = "",
        string settingsFileName = "appsettings.json")
    {
        this.withConfiguration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile(settingsFileName, false)
            .AddJsonFile($@"{Environment.MachineName}.{settingsFileName}", true)
            .AddEnvironmentVariables(environmentVariablesPrefix)
            .Build();

        return this;
    }

    public TestEnvironmentBuilder WithSecondaryDatabases(string[] databaseNames)
    {
        this.withSecondaryDatabases = databaseNames;

        return this;
    }

    public TestEnvironmentBuilder WithConnectionStringName(string name)
    {
        this.withConnectionStringName = name;

        return this;
    }

    public TestEnvironmentBuilder WithServiceProviderBuildFunc(Func<IConfiguration, IServiceCollection, IServiceCollection> buildFunc)
    {
        this.withServiceProviderBuildFunc = buildFunc;

        return this;
    }

    public TestEnvironmentBuilder WithServiceProviderAfterBuildAction(Action<IServiceProvider> afterBuildAction)
    {
        this.withServiceProvAfterBuildAction = afterBuildAction;

        return this;
    }

    public TestEnvironmentBuilder WithDatabaseGenerator<T>() where T: ITestDatabaseGenerator
    {
        this.withDatabaseGenerator = typeof(T);

        return this;
    }

    public TestEnvironment Build()
    {
        if (this.withConfiguration == null)
        {
            this.WithDefaultConfiguration();
        }

        if (this.withDatabaseGenerator == null)
        {
            throw new ArgumentException("Please provide DatabaseGenerator via '.WithDatabaseGenerator()'");
        }

        if (this.withServiceProviderBuildFunc == null)
        {
            throw new ArgumentException("Please provide ServiceProvider build function via '.WithServiceProviderBuildFunc()'");
        }

        var serviceProviderPool = this.GetServiceProviderPool(
            this.withConfiguration,
            this.withServiceProviderBuildFunc,
            this.withServiceProvAfterBuildAction,
            this.withDatabaseGenerator,
            this.withConnectionStringName ?? "DefaultConnection",
            this.withSecondaryDatabases);

        return new TestEnvironment(
            serviceProviderPool,
            this.GetAssemblyInitializeAndCleanup(serviceProviderPool));
    }

    protected virtual AssemblyInitializeAndCleanup GetAssemblyInitializeAndCleanup(
        IServiceProviderPool serviceProviderPool) =>
        new AssemblyInitializeAndCleanup(
            serviceProviderPool.Get,
            serviceProviderPool.Release);

    protected virtual IServiceProvider BuildServiceProvider(IServiceCollection serviceCollection) =>
        serviceCollection
            .ValidateDuplicateDeclaration()
            .BuildServiceProvider(
            new ServiceProviderOptions
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

    protected virtual IServiceProviderPool GetServiceProviderPool(
        IConfiguration rootConfiguration,
        Func<IConfiguration, IServiceCollection, IServiceCollection> serviceProviderBuildFunc,
        Action<IServiceProvider> serviceProviderAfterBuildAction,
        Type databaseGenerator,
        string connectionStringName,
        string[] secondaryDatabases) =>
            new ServiceProviderPool(
                () => this.ServiceProviderGenerationFunc(
                    serviceProviderBuildFunc,
                    serviceProviderAfterBuildAction,
                    rootConfiguration,
                    databaseGenerator,
                    connectionStringName,
                    secondaryDatabases),
                rootConfiguration.GetValue<bool>("TestsParallelize"),
                rootConfiguration.GetValue<bool>("DisableServiceProviderPoolLimiter"));

    private IServiceProvider ServiceProviderGenerationFunc(
            Func<IConfiguration, IServiceCollection, IServiceCollection> serviceProviderBuildFunc,
            Action<IServiceProvider> serviceProviderAfterBuildAction,
            IConfiguration rootConfiguration,
            Type databaseGenerator,
            string connectionStringName,
            string[] secondaryDatabases)
    {
        var configUtil = new ConfigUtil(rootConfiguration);
        var databaseContextSettings = new DatabaseContextSettings(
            connectionStringName,
            secondaryDatabases);

        var databaseContext = new DatabaseContext(
            configUtil,
            Options.Create(databaseContextSettings));

        var cfg = rootConfiguration.BuildFromRootWithConnectionStrings(databaseContext);

        var environmentServices = new ServiceCollection();
        serviceProviderBuildFunc.Invoke(cfg, environmentServices);
        environmentServices.TryAddSingleton(databaseContextSettings);
        environmentServices.TryAddSingleton<IDatabaseContext>(databaseContext);
        environmentServices.AddSingleton<IConfiguration>(cfg)
                           .AddSingleton<ConfigUtil>()
                           .AddSingleton(typeof(ITestDatabaseGenerator), databaseGenerator);

        var environmentServiceProvider = this.BuildServiceProvider(environmentServices);

        serviceProviderAfterBuildAction?.Invoke(environmentServiceProvider);

        return environmentServiceProvider;
    }
}

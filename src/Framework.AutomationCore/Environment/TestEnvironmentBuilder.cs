using Automation.Extensions;
using Automation.Settings;
using Automation.Utils.DatabaseUtils;
using Automation.Utils.DatabaseUtils.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Automation;

public class TestEnvironmentBuilder
{
    private IConfiguration? withConfiguration;

    private Func<IConfiguration, IServiceCollection, IServiceCollection>? withServiceProviderBuildFunc;

    private Action<IServiceProvider> withServiceProvAfterBuildAction = _ => { };

    private Action<AutomationFrameworkSettings> withAutomationFrameworkSettings = _ => { };

    private Type? withDatabaseGenerator;

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

    public TestEnvironmentBuilder WithAutomationFrameworkSettings(Action<AutomationFrameworkSettings> settings)
    {
        this.withAutomationFrameworkSettings = settings;

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
            this.withConfiguration!,
            this.withServiceProviderBuildFunc,
            this.withServiceProvAfterBuildAction,
            this.withDatabaseGenerator,
            this.withAutomationFrameworkSettings);

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
        Action<AutomationFrameworkSettings> settingsAction) =>
            new ServiceProviderPool(
                () => this.ServiceProviderGenerationFunc(
                    serviceProviderBuildFunc,
                    serviceProviderAfterBuildAction,
                    rootConfiguration,
                    databaseGenerator,
                    settingsAction),
                rootConfiguration.GetValue<bool>("TestsParallelize"),
                rootConfiguration.GetValue<bool>("DisableServiceProviderPoolLimiter"));

    private IServiceProvider ServiceProviderGenerationFunc(
            Func<IConfiguration, IServiceCollection, IServiceCollection> serviceProviderBuildFunc,
            Action<IServiceProvider> serviceProviderAfterBuildAction,
            IConfiguration rootConfiguration,
            Type databaseGenerator,
            Action<AutomationFrameworkSettings> settingsAction)
    {
        var settings = GetSettings(rootConfiguration, settingsAction);
        var databaseContext = new DatabaseContext(rootConfiguration, settings);

        var cfg = rootConfiguration.BuildFromRootWithConnectionStrings(databaseContext);

        var environmentServices = new ServiceCollection();
        serviceProviderBuildFunc.Invoke(cfg, environmentServices);
        environmentServices.TryAddSingleton(settings);
        environmentServices.TryAddSingleton<IDatabaseContext>(databaseContext);
        environmentServices.AddSingleton<IConfiguration>(cfg)
                           .AddSingleton(typeof(ITestDatabaseGenerator), databaseGenerator);

        var environmentServiceProvider = this.BuildServiceProvider(environmentServices);

        serviceProviderAfterBuildAction(environmentServiceProvider);

        return environmentServiceProvider;
    }

    private static IOptions<AutomationFrameworkSettings> GetSettings(IConfiguration configuration, Action<AutomationFrameworkSettings> action)
    {
        var settings = new AutomationFrameworkSettings();
        configuration.GetSection(nameof(AutomationFrameworkSettings)).Bind(settings);
        action(settings);

        return Options.Create(settings);
    }
}

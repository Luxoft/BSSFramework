using Automation.Utils.DatabaseUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Automation;

public class TestEnvironmentBuilder
{
    private IConfiguration configuration;
    private Func<IConfiguration, IServiceCollection, IServiceCollection> serviceProviderBuildFunc;
    private Action<IServiceProvider> serviceProviderAfterBuildAction;
    private Type databaseGenerator;
    private string connectionStringName;
    private string[] secondaryDatabases;

    public TestEnvironmentBuilder WithConfiguration(IConfiguration rootConfiguration)
    {
        this.configuration = rootConfiguration;

        return this;
    }

    public TestEnvironmentBuilder WithDefaultConfiguration(string environmentVariablesPrefix = "")
    {
        this.configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($@"appsettings.json", false)
            .AddJsonFile($@"{Environment.MachineName}.appsettings.json", true)
            .AddEnvironmentVariables(environmentVariablesPrefix)
            .Build();

        return this;
    }

    public TestEnvironmentBuilder WithSecondaryDatabases(string[] databaseNames)
    {
        this.secondaryDatabases = databaseNames;

        return this;
    }

    public TestEnvironmentBuilder WithConnectionStringName(string name)
    {
        this.connectionStringName = name;

        return this;
    }

    public TestEnvironmentBuilder WithServiceProviderBuildFunc(Func<IConfiguration, IServiceCollection, IServiceCollection> buildFunc)
    {
        this.serviceProviderBuildFunc = buildFunc;

        return this;
    }

    public TestEnvironmentBuilder WithServiceProviderAfterBuildAction(Action<IServiceProvider> afterBuildAction)
    {
        this.serviceProviderAfterBuildAction = afterBuildAction;

        return this;
    }

    public TestEnvironmentBuilder WithDatabaseGenerator<T>() where T: TestDatabaseGenerator
    {
        this.databaseGenerator = typeof(T);

        return this;
    }

    public TestEnvironment Build()
    {
        if (this.configuration == null)
        {
            this.WithDefaultConfiguration();
        }

        if (this.databaseGenerator == null)
        {
            throw new ArgumentException("Please provide DatabaseGenerator via '.WithDatabaseGenerator()'");
        }

        if (this.serviceProviderBuildFunc == null)
        {
            throw new ArgumentException("Please provide ServiceProvider build function via '.WithServiceProviderBuildFunc()'");
        }

        return new TestEnvironment(
            this.configuration,
            this.serviceProviderBuildFunc,
            this.serviceProviderAfterBuildAction,
            this.databaseGenerator,
            this.connectionStringName ?? "DefaultConnection",
            this.secondaryDatabases);
    }
}

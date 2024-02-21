using System.Text.RegularExpressions;
using Automation.Enums;
using Microsoft.Extensions.Configuration;

namespace Automation.Utils;

public class ConfigUtil
{
    private readonly Lazy<string> tempFolderLazy;
    private readonly Lazy<string> dataDirectory;
    private readonly Lazy<bool> useLocalDbLazy;
    private readonly Lazy<bool> testsParallelizeLazy;
    private readonly Lazy<TestRunMode> testRunModeLazy;
    private readonly IConfiguration configuration;
    private readonly Lazy<string> databaseCollation;

    private static readonly string LocalDbInstanceName;

    static ConfigUtil() => LocalDbInstanceName = $"Test_{TextRandomizer.RandomString(10)}";

    public ConfigUtil(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.useLocalDbLazy = new Lazy<bool>(() => this.configuration.GetValue<bool>("UseLocalDb"));
        this.testsParallelizeLazy = new Lazy<bool>(() => this.configuration.GetValue<bool>("TestsParallelize"));
        this.databaseCollation = new Lazy<string>(() => this.configuration.GetValue<string>("DatabaseCollation"));
        this.testRunModeLazy = new Lazy<TestRunMode>(
            () => this.configuration.GetValue<TestRunMode>("TestRunMode", TestRunMode.DefaultRunModeOnEmptyDatabase));

        var serverRootFolderLazy = new Lazy<string>(() => this.configuration.GetValue<string>("TestRunServerRootFolder"));
        this.dataDirectory = new Lazy<string>(
            () => this.CheckDirectoryAndCreateIfNotExists(Path.Combine(serverRootFolderLazy.Value, "data")));
        this.tempFolderLazy = new Lazy<string>(
            () => this.CheckDirectoryAndCreateIfNotExists(Path.Combine(serverRootFolderLazy.Value, "temp")));
    }

    public string ComputerName => Environment.MachineName;

    public string DbDataDirectory => this.dataDirectory.Value;

    public TestRunMode TestRunMode => this.testRunModeLazy.Value;

    public string TempFolder => this.tempFolderLazy.Value;

    public bool UseLocalDb => this.useLocalDbLazy.Value;

    public bool TestsParallelize => this.testsParallelizeLazy.Value;

    public string DatabaseCollation => this.databaseCollation.Value;

    public string GetConnectionString(string connectionStringName)
    {
        var connectionString = this.configuration.GetConnectionString(connectionStringName);

        if (this.UseLocalDb)
        {
            connectionString = this.GetLocalDbConnectionString(connectionString, LocalDbInstanceName);
        }

        return connectionString;
    }

    private string GetLocalDbConnectionString(string connectionString, string instanceName)
        => DataSourceRegex.Replace(connectionString, $"Data Source=(localdb)\\{instanceName}");

    private static readonly Regex DataSourceRegex = new Regex("Data Source=([^;]*)", RegexOptions.Compiled);

    public string GetDataSource(string connectionString) =>
        DataSourceRegex.Matches(connectionString).First().Value;

    private string CheckDirectoryAndCreateIfNotExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path!);
        }

        return path;
    }
}

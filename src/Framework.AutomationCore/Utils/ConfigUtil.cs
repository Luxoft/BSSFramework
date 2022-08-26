using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Automation.Enums;
using Microsoft.Extensions.Configuration;

namespace Automation.Utils;

public class ConfigUtil
{
    private readonly Lazy<string> ServerRootFolderLazy;
    private readonly Lazy<string> TempFolderLazy;
    private readonly Lazy<string> DataDirectory;
    private readonly Lazy<bool> UseLocalDbLazy;
    private readonly Lazy<bool> TestsParallelizeLazy;
    private readonly Lazy<string> SystemNameLazy;
    private readonly Lazy<TestRunMode> TestRunModeLazy;

    public ConfigUtil(IConfiguration appsettings)
    {
        this.Configuration = appsettings;
        ServerRootFolderLazy = new Lazy<string>(() => this.Configuration["TestRunServerRootFolder"]);
        TempFolderLazy = new Lazy<string>(
            () =>
            {
                var path = Configuration["TempFolder"];

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            });
        DataDirectory = new Lazy<string>(
            () =>
            {
                if (!Directory.Exists(Path.Combine(ServerRootFolderLazy.Value, "data")))
                {
                    Directory.CreateDirectory(Path.Combine(ServerRootFolderLazy.Value, "data"));
                }

                return Path.Combine(ServerRootFolderLazy.Value, "data");
            });
        UseLocalDbLazy = new Lazy<bool>(() => bool.Parse(Configuration["UseLocalDb"]));

        this.TestsParallelizeLazy = new Lazy<bool>(() => bool.Parse(Configuration["TestsParallelize"]));
        SystemNameLazy = new Lazy<string>(() => Configuration["SystemName"]);
        TestRunModeLazy = new Lazy<TestRunMode>(
            () =>
            {
                if (!Enum.TryParse(Configuration["TestRunMode"], out TestRunMode runMode))
                {
                    runMode = TestRunMode.DefaultRunModeOnEmptyDatabase;
                }

                return runMode;
            });
    }

    private IConfiguration Configuration;

    public string ComputerName => Environment.MachineName;

    public string DbDataDirectory => DataDirectory.Value;

    public TestRunMode TestRunMode => TestRunModeLazy.Value;

    public string TempFolder => TempFolderLazy.Value;

    public bool UseLocalDb => UseLocalDbLazy.Value;

    public bool TestsParallelize => this.TestsParallelizeLazy.Value;

    public string SystemName => SystemNameLazy.Value;

    public string GetConnectionString(string connectionStringName)
    {
        var connectionString = Configuration.GetConnectionString(connectionStringName);

        if (this.UseLocalDb)
        {
            var instanceName = $"{this.SystemName}{TextRandomizer.RandomString(5)}";

            connectionString = this.GetLocalDbConnectionString(connectionString, instanceName);
        }

        return connectionString;
    }

    private string GetLocalDbConnectionString(string connectionString, string instanceName)
        => DataSourceRegex.Replace(connectionString, $"(localdb)\\{instanceName}");

    private static readonly Regex DataSourceRegex = new Regex("Data Source=([.\\\\w].*);", RegexOptions.Compiled);

    public string GetDataSource(string connectionString) =>
        DataSourceRegex.Matches(connectionString).First().Value;
}

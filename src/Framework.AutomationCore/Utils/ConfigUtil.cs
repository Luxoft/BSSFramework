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

    public ConfigUtil(IConfiguration configuration)
    {
        this.Configuration = configuration;
        this.ServerRootFolderLazy = new Lazy<string>(() => this.Configuration["TestRunServerRootFolder"]);
        this.TempFolderLazy = new Lazy<string>(
            () =>
            {
                var path = this.Configuration["TempFolder"];

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            });
        this.DataDirectory = new Lazy<string>(
            () =>
            {
                if (!Directory.Exists(Path.Combine(this.ServerRootFolderLazy.Value, "data")))
                {
                    Directory.CreateDirectory(Path.Combine(this.ServerRootFolderLazy.Value, "data"));
                }

                return Path.Combine(this.ServerRootFolderLazy.Value, "data");
            });
        this.UseLocalDbLazy = new Lazy<bool>(() => bool.Parse(this.Configuration["UseLocalDb"]));

        this.TestsParallelizeLazy = new Lazy<bool>(() => bool.Parse(this.Configuration["TestsParallelize"]));
        this.SystemNameLazy = new Lazy<string>(() => this.Configuration["SystemName"]);
        this.TestRunModeLazy = new Lazy<TestRunMode>(
            () =>
            {
                if (!Enum.TryParse(this.Configuration["TestRunMode"], out TestRunMode runMode))
                {
                    runMode = TestRunMode.DefaultRunModeOnEmptyDatabase;
                }

                return runMode;
            });
    }

    private IConfiguration Configuration;

    public string ComputerName => Environment.MachineName;

    public string DbDataDirectory => this.DataDirectory.Value;

    public TestRunMode TestRunMode => this.TestRunModeLazy.Value;

    public string TempFolder => this.TempFolderLazy.Value;

    public bool UseLocalDb => this.UseLocalDbLazy.Value;

    public bool TestsParallelize => this.TestsParallelizeLazy.Value;

    public string SystemName => this.SystemNameLazy.Value;

    public string GetConnectionString(string connectionStringName)
    {
        var connectionString = this.Configuration.GetConnectionString(connectionStringName);

        if (this.UseLocalDb)
        {
            var instanceName = $"{this.SystemName}{TextRandomizer.RandomString(5)}";

            connectionString = this.GetLocalDbConnectionString(connectionString, instanceName);
        }

        return connectionString;
    }

    private string GetLocalDbConnectionString(string connectionString, string instanceName)
        => DataSourceRegex.Replace(connectionString, $"Data Source=(localdb)\\{instanceName}");

    private static readonly Regex DataSourceRegex = new Regex("Data Source=([^;]*)", RegexOptions.Compiled);

    public string GetDataSource(string connectionString) =>
        DataSourceRegex.Matches(connectionString).First().Value;
}

using System;
using System.IO;

using Automation.Enums;
namespace Automation.Utils;

public static class ConfigUtil
{
    private static readonly Lazy<string> DataDirectory = new Lazy<string>(
        () =>
        {
            if (!Directory.Exists(Path.Combine(ServerRootFolder.Value, "data")))
            {
                Directory.CreateDirectory(Path.Combine(ServerRootFolder.Value, "data"));
            }

            return Path.Combine(ServerRootFolder.Value, "data");
        });

    private static readonly Lazy<string> ServerRootFolder = new Lazy<string>(() => AppSettings.Default["TestRunServerRootFolder"]);

    private static readonly Lazy<string> TempFolderLazy = new Lazy<string>(
        () =>
        {
            var path = AppSettings.Default["TempFolder"];

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        });

    private static readonly Lazy<bool> UseLocalDbLazy = new Lazy<bool>(() => bool.Parse(AppSettings.Default["UseLocalDb"]));

    private static readonly Lazy<bool> RandomizeDatabaseNameLazy = new Lazy<bool>(() => bool.Parse(AppSettings.Default["RandomizeDatabaseName"]));

    private static readonly Lazy<string> SystemNameLazy = new Lazy<string>(() => AppSettings.Default["SystemName"]);

    private static readonly Lazy<TestRunMode> TestRunModeLazy = new Lazy<TestRunMode>(
        () =>
        {
            if (!Enum.TryParse(AppSettings.Default["TestRunMode"], out TestRunMode runMode))
            {
                runMode = TestRunMode.DefaultRunModeOnEmptyDatabase;
            }

            return runMode;
        });

    public static string ComputerName => Environment.MachineName;

    public static string UserName => Environment.UserName;

    public static string DbDataDirectory => DataDirectory.Value;

    public static TestRunMode TestRunMode => TestRunModeLazy.Value;

    public static string TempFolder => TempFolderLazy.Value;

    public static bool UseLocalDb => UseLocalDbLazy.Value;

    public static bool RandomizeDatabaseName => RandomizeDatabaseNameLazy.Value;

    public static string SystemName => SystemNameLazy.Value;
}
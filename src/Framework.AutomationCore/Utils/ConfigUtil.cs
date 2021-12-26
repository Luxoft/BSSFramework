using System;
using System.IO;
using System.Text.RegularExpressions;

using Automation.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace Automation.Utils
{
    public static class ConfigUtil
    {
        private static readonly Lazy<string> DataDirectory = new Lazy<string>(
            () =>
            {
                return string.Empty;

                if (!Directory.Exists(Path.Combine(ServerRootFolder.Value, "DATA")))
                {
                    Directory.CreateDirectory(Path.Combine(ServerRootFolder.Value, "DATA"));
                }

                return Path.Combine(ServerRootFolder.Value, "DATA");
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

        private static readonly Lazy<Server> ServerLazy =
            new Lazy<Server>(() => new Server(new ServerConnection(new SqlConnection(CutInitialCatalog(ConnectionString)))));

        private static readonly Lazy<bool> UseLocalDbLazy = new Lazy<bool>(() => bool.Parse(AppSettings.Default["UseLocalDb"]));

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

        public static string ConnectionString;

        public static string InstanceName;

        public static Server Server
        {
            get
            {
                var srv = ServerLazy.Value;

                srv.Refresh();

                return srv;
            }
        }

        public static string ComputerName => Environment.MachineName;

        public static string UserName => Environment.UserName;

        public static string DbDataDirectory => DataDirectory.Value;

        public static TestRunMode TestRunMode => TestRunModeLazy.Value;

        public static string TempFolder => TempFolderLazy.Value;

        public static bool LocalServer => Server.NetName.Equals(ComputerName, StringComparison.InvariantCultureIgnoreCase);

        public static bool UseLocalDb => UseLocalDbLazy.Value;

        public static string SystemName => SystemNameLazy.Value;
        public static Server Lazy => ServerLazy.Value;

        private static string CutInitialCatalog(string inputConnectionString) =>
            Regex.Replace(inputConnectionString, "Initial Catalog=(\\w+);", "");
    }
}

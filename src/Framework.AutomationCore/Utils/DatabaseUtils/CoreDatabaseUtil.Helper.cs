using System;
using System.Collections.Specialized;
using System.IO;

using Microsoft.SqlServer.Management.Smo;

namespace Automation.Utils
{
    public static partial class CoreDatabaseUtil
    {
        private static readonly Lazy<string> FileNameLazy = new Lazy<string>(() => $"{DatabaseName}{DateTime.Now:yyMMddHHmmss}");

        public static string DatabaseName { get; set; }

        public static string BackupNamePrefix => $"{ConfigUtil.SystemName}_{ConfigUtil.UserName}_";

        private static StringCollection SourceFiles => new StringCollection { SourceDataPath, SourceLogPath };

        private static string SourceDataPath => ToWorkPath(SourceDataFile);

        private static string SourceLogPath => ToWorkPath(SourceLogFile);

        public static string CopyDataPath => ToWorkPath(CopyDataFile);

        private static string CopyLogPath => ToWorkPath(CopyLogFile);

        private static string CopyDataFile => BackupNamePrefix + $"{DatabaseName}.mdf";

        private static string CopyLogFile => BackupNamePrefix + $"{DatabaseName}_log.ldf";

        private static string SourceDataFile => $"{FileName}.mdf";

        private static string SourceLogFile => $"{FileName}_log.ldf";

        private static string FileName => FileNameLazy.Value;

        private static Table GetTable(string databaseName, string tableName)
        {
            var database = GetDatabase(databaseName);

            return database?.Tables[tableName];
        }

        public static long TableRowCount(string databaseName, string tableName) => GetTable(databaseName, tableName)?.RowCount ?? 0;

        private static string ToWorkPath(string fileName) => Path.Combine(ConfigUtil.DbDataDirectory, fileName);

        private static void SetModeRestrictedUser(string databaseName)
        {
            if (GetDatabase(databaseName) == null)
            {
                return;
            }

            try
            {
                ConfigUtil.Server.KillAllProcesses(databaseName);
            }
            catch (FailedOperationException)
            {
                ExecuteSql($"ALTER DATABASE [{databaseName}] SET RESTRICTED_USER WITH ROLLBACK IMMEDIATE", "master");
            }
        }

        private static void SetModeMultiUser(Database database) => ExecuteSql($"ALTER DATABASE [{database.Name}] SET MULTI_USER", "master");

        private static Database GetDatabase(string name) => ConfigUtil.Server.Databases.Contains(name) ? ConfigUtil.Server.Databases[name] : null;
    }
}

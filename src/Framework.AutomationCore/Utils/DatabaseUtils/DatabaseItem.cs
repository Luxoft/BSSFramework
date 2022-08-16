using System;
using System.IO;
using System.Linq;
using Automation.Utils.DatabaseUtils.Interfaces;
using Microsoft.Data.SqlClient;

namespace Automation.Utils.DatabaseUtils
{
    public class DatabaseItem : IDatabaseItem
    {
        private SqlConnectionStringBuilder builder;

        public DatabaseItem(string connectionString, string initialCatalog = null)
        {
            this.builder = new SqlConnectionStringBuilder(connectionString);
            initialCatalog ??= this.builder.InitialCatalog;
            this.DatabaseName = ConfigUtil.RandomizeDatabaseName
                ? TextRandomizer.RandomString(initialCatalog, 14)
                : initialCatalog;

            if (ConfigUtil.UseLocalDb)
            {
                this.SetLocalDbInstance(
                    ConfigUtil.RandomizeDatabaseName
                        ? TextRandomizer.RandomString(ConfigUtil.SystemName, 14)
                        : initialCatalog);
            }

            var fileName = $"{this.InstanceName}_{this.DatabaseName}_{TextRandomizer.RandomString(5)}";

            this.CopyDataPath = ToCopyDataPath(initialCatalog);
            this.CopyLogPath = ToCopyLogPath(initialCatalog);
            this.SourceDataPath = ToSourceDataPath(fileName);
            this.SourceLogPath = ToSourceLogPath(fileName);
            this.builder.InitialCatalog = this.DatabaseName;
        }

        public string DataSource => this.builder.DataSource;

        public string InitialCatalog => this.builder.InitialCatalog;

        public string UserId => this.builder.UserID;

        public string Password => this.builder.Password;

        public bool IntegratedSecurity => this.builder.IntegratedSecurity;

        public string ConnectionString => this.builder.ConnectionString;

        public string InstanceName => this.builder.DataSource.Split('\\').LastOrDefault();

        public string DatabaseName { get; }
        public string CopyDataPath { get; }
        public string CopyLogPath { get; }
        public string SourceDataPath { get; }
        public string SourceLogPath { get; }

        private static string ToSourceDataPath(string fileName) => ToWorkPath(SourceDataFile(fileName));

        private static string ToSourceLogPath(string fileName) => ToWorkPath(SourceLogFile(fileName));

        private static string ToCopyDataPath(string initialCatalog) => ToWorkPath(CopyDataFile(initialCatalog));

        private static string ToCopyLogPath(string initialCatalog) => ToWorkPath(CopyLogFile(initialCatalog));

        private static string CopyDataFile(string initialCatalog) => CoreDatabaseUtil.BackupNamePrefix + $"{initialCatalog}.mdf";

        private static string CopyLogFile(string initialCatalog) => CoreDatabaseUtil.BackupNamePrefix + $"{initialCatalog}_log.ldf";

        private static string SourceDataFile(string fileName) => $"{fileName}.mdf";

        private static string SourceLogFile(string fileName) => $"{fileName}_log.ldf";

        private static string ToWorkPath(string fileName) => Path.Combine(ConfigUtil.DbDataDirectory, fileName);

        private void SetLocalDbInstance(string instanceName) => this.builder.DataSource = $"(localdb)\\{instanceName}";

        public bool IsLocalDb => this.builder.DataSource.StartsWith("(localdb)", StringComparison.InvariantCultureIgnoreCase);
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseDatabaseUtil.cs" company="">
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Automation.Utils.Utils.DatabaseUtils;

using Microsoft.SqlServer.Management.Smo;

namespace Automation.Utils
{
    public abstract class BaseDatabaseUtil
    {
        protected abstract IEnumerable<string> TestServers { get; }

        protected ConnectionSettings ConnectionSettings { get; }

        protected BaseDatabaseUtil()
        {
            this.ConnectionSettings = new ConnectionSettings();
            CoreDatabaseUtil.DatabaseName = this.ConnectionSettings.InitialCatalog;
        }

        public void CreateDatabase() => CoreDatabaseUtil.ReCreate();

        public virtual void DropDatabase() => CoreDatabaseUtil.Drop();

        public virtual void ExecuteInsertsForDatabases() =>
            CoreDatabaseUtil.ExecuteSqlFromFolder(@"__Support/Scripts", CoreDatabaseUtil.DatabaseName);

        public void DropAllDatabases()
        {
            ConfigUtil.Server
                .Databases.Cast<Database>()
                .Where(x => x.Name.StartsWith(CoreDatabaseUtil.DatabaseName))
                .ToList()
                .ForEach(CoreDatabaseUtil.Drop);

            CoreDatabaseUtil.DeleteLocalDb();
        }

        public void CopyDetachedFiles() => CoreDatabaseUtil.CopyDetachedFiles();

        public void AttachDatabase() => CoreDatabaseUtil.AttachDatabase();

        public abstract void GenerateDatabases();

        public void DeleteDetachedFiles() =>
            Directory.GetFiles(ConfigUtil.DbDataDirectory)
                .Where(i => i.Contains(CoreDatabaseUtil.BackupNamePrefix))
                .ToList()
                .ForEach(File.Delete);

        public void CheckAndCreateDetachedFiles()
        {
            if (!new FileInfo(CoreDatabaseUtil.CopyDataPath).Exists)
            {
                this.CreateDatabase();
                this.GenerateDatabases();
                this.ExecuteInsertsForDatabases();
                this.GenerateTestData();
                this.CopyDetachedFiles();
            }
        }

        public abstract void CheckTestDatabase();

        public void CheckServerAllowed()
        {
            if (!ConfigUtil.LocalServer)
            {
                if (!this.TestServers.Select(s => s.ToUpper())
                    .ToList()
                    .Contains(ConfigUtil.Server.NetName.ToUpper()))
                {
                    throw new Exception(
                        $"Server name {ConfigUtil.Server.NetName} is not specified in allowed list of test servers: {string.Join(", ", this.TestServers.Select(s => s.ToUpper()).ToList())}");
                }
            }
        }

        public abstract void GenerateTestData();
    }
}

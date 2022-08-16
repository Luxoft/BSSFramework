using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Automation.Utils.DatabaseUtils.Interfaces;
using Microsoft.SqlServer.Management.Smo;

namespace Automation.Utils.DatabaseUtils
{
    public abstract class BaseDatabaseUtil : IDatabaseUtil
    {
        protected abstract IEnumerable<string> TestServers { get; }

        public IDatabaseContext DatabaseContext { get; }

        protected BaseDatabaseUtil(IDatabaseContext databaseContext)
        {
            this.DatabaseContext = databaseContext;
        }
        public void CreateDatabase() => this.DatabaseContext.ReCreate();

        public virtual void DropDatabase()
        {
            this.DatabaseContext.Drop();
        }

        public virtual void DropAllDatabases()
        {
            this.DatabaseContext.Server.Databases.Cast<Database>()
                .Where(x => x.Name.Equals(this.DatabaseContext.MainDatabase.InitialCatalog))
                .ToList()
                .ForEach(x => x.Drop());
        }

        public virtual void ExecuteInsertsForDatabases()
        {
            DatabaseUtils.CoreDatabaseUtil.ExecuteSqlFromFolder(
                this.DatabaseContext.MainDatabase.ConnectionString,
                @"__Support\Scripts",
                this.DatabaseContext.MainDatabase.DatabaseName);
        }

        public void CopyDetachedFiles() => this.DatabaseContext.CopyDetachedFiles();

        public void AttachDatabase() => this.DatabaseContext.AttachDatabase();

        public abstract void GenerateDatabases();

        public void DeleteDetachedFiles() =>
            Directory.GetFiles(ConfigUtil.DbDataDirectory)
                .Where(i => i.Contains(CoreDatabaseUtil.BackupNamePrefix))
                .ToList()
                .ForEach(File.Delete);

        public void CheckAndCreateDetachedFiles()
        {
            if (!new FileInfo(this.DatabaseContext.MainDatabase.CopyDataPath).Exists)
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
            if (!this.DatabaseContext.Server.NetName.Equals(ConfigUtil.ComputerName, StringComparison.InvariantCultureIgnoreCase))
            {
                if (!this.TestServers.Select(s => s.ToUpper())
                    .ToList()
                    .Contains(this.DatabaseContext.Server.NetName.ToUpper()))
                {
                    throw new Exception(
                        $"Server name {this.DatabaseContext.Server.NetName} is not specified in allowed list of test servers: {string.Join(", ", this.TestServers.Select(s => s.ToUpper()).ToList())}");
                }
            }
        }

        public abstract void GenerateTestData();
    }
}

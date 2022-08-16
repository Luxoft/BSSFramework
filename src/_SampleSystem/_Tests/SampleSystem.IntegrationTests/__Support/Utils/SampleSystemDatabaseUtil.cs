using System;
using System.Collections.Generic;

using Automation.Utils;
using Automation.Utils.DatabaseUtils;
using Automation.Utils.DatabaseUtils.Interfaces;
using Framework.DomainDriven.DBGenerator;

using Microsoft.Extensions.DependencyInjection;
using SampleSystem.DbGenerate;
using SampleSystem.IntegrationTests.__Support;
using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.WebApiCore;

using WorkflowCore.Interface;

namespace SampleSystem.IntegrationTests.Support.Utils
{
    public class SampleSystemDatabaseUtil : BaseDatabaseUtil
    {
        protected override IEnumerable<string> TestServers => new List<string> { "." };

        public SampleSystemDatabaseUtil(IDatabaseContext databaseContext) : base(databaseContext)
        {
        }

        public override void GenerateDatabases()
        {
            new DbGeneratorTest().GenerateAllDB(
                this.DatabaseContext.MainDatabase.DataSource,
                mainDatabaseName: this.DatabaseContext.MainDatabase.DatabaseName,
                credential: UserCredential.Create(
                    this.DatabaseContext.MainDatabase.UserId,
                    this.DatabaseContext.MainDatabase.Password));
        }

        public override void CheckTestDatabase()
        {
            if (this.DatabaseContext.Server.TableRowCount(this.DatabaseContext.MainDatabase.DatabaseName, "Location") > 100)
            {
                throw new Exception(
                    "Location row count more than 100. Please ensure that you run tests in Test Environment. If you want to run tests in the environment, please delete all Location rows (Location table) manually and rerun tests.");
            }
        }

        public override void GenerateTestData() => new TestDataInitialize(this.DatabaseContext).TestData();

        public override void ExecuteInsertsForDatabases()
        {
            base.ExecuteInsertsForDatabases();

            CoreDatabaseUtil.ExecuteSqlFromFolder(this.DatabaseContext.MainDatabase.ConnectionString, @"__Support/Scripts/Authorization", this.DatabaseContext.MainDatabase.DatabaseName);
            CoreDatabaseUtil.ExecuteSqlFromFolder(this.DatabaseContext.MainDatabase.ConnectionString,@"__Support/Scripts/Configuration", this.DatabaseContext.MainDatabase.DatabaseName);

            this.GenerateWorkflowCoreDataBase();

            CoreDatabaseUtil.ExecuteSqlFromFolder(this.DatabaseContext.MainDatabase.ConnectionString,@"__Support/Scripts/SampleSystem", this.DatabaseContext.MainDatabase.DatabaseName);

            new BssFluentMigrator(this.DatabaseContext.MainDatabase.ConnectionString, typeof(InitNumberInDomainObjectEventMigration).Assembly).Migrate();
        }

        private void GenerateWorkflowCoreDataBase()
        {
            AppSettings.Initialize(nameof(SampleSystem) + "_");

            var serviceProvider = new ServiceCollection()
                .AddWorkflowCore(this.DatabaseContext.MainDatabase.ConnectionString)
                .BuildServiceProvider();

            var workflowHost = serviceProvider.GetRequiredService<IWorkflowHost>();

            workflowHost.Start();
            workflowHost.Stop();
        }
    }
}

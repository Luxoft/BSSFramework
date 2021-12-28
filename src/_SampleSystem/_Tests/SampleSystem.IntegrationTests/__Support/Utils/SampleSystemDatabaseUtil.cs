﻿using System;
using System.Collections.Generic;

using Automation.Utils;
using Automation.Utils.Utils.DatabaseUtils;

using Framework.DomainDriven.DBGenerator;

using SampleSystem.DbGenerate;
using SampleSystem.DbMigrator;
using SampleSystem.IntegrationTests.__Support;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests.Support.Utils
{
    public class SampleSystemDatabaseUtil : BaseDatabaseUtil
    {
        protected override IEnumerable<string> TestServers => new List<string> { "." };

        public new ConnectionSettings ConnectionSettings => base.ConnectionSettings;

        public override void GenerateDatabases()
        {
            new DbGeneratorTest().GenerateAllDB(
                                                this.ConnectionSettings.DataSource,
                                                credential: UserCredential.Create(this.ConnectionSettings.UserId,this.ConnectionSettings.Password));
        }

        public override void CheckTestDatabase()
        {
            if (CoreDatabaseUtil.TableRowCount(CoreDatabaseUtil.DatabaseName, "Location") > 100)
            {
                throw new Exception(
                    "Location row count more than 100. Please ensure that you run tests in Test Environment. If you want to run tests in the environment, please delete all Location rows (Location table) manually and rerun tests.");
            }
        }

        public override void GenerateTestData() => new TestDataInitialize().TestData();

        public override void ExecuteInsertsForDatabases()
        {
            base.ExecuteInsertsForDatabases();

            CoreDatabaseUtil.ExecuteSqlFromFolder(@"__Support\Scripts\Authorization", CoreDatabaseUtil.DatabaseName);
            CoreDatabaseUtil.ExecuteSqlFromFolder(@"__Support\Scripts\Configuration", CoreDatabaseUtil.DatabaseName);
            CoreDatabaseUtil.ExecuteSqlFromFolder(@"__Support\Scripts\Workflow", CoreDatabaseUtil.DatabaseName);
            CoreDatabaseUtil.ExecuteSqlFromFolder(@"__Support\Scripts\SampleSystem", CoreDatabaseUtil.DatabaseName);

            new BssFluentMigrator(this.ConnectionSettings.ConnectionString, typeof(InitNumberInDomainObjectEventMigration).Assembly).Migrate();
        }
    }
}

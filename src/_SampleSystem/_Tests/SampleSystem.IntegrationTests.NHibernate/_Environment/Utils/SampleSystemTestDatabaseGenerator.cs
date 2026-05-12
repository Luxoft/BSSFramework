//using Framework.AutomationCore.Settings;
//using Framework.AutomationCore.TestingProvider;
//using Framework.AutomationCore.Utils.DatabaseUtils;
////using Framework.AutomationCore.Utils.DatabaseUtils.Interfaces;
//using Framework.Database.NHibernate.DBGenerator;

//using Microsoft.Extensions.Options;

//using SampleSystem.DbGenerate.NHibernate;
////using SampleSystem.IntegrationTests.__Support.FluentMigration;
////using SampleSystem.IntegrationTests.__Support.TestData;
//using SampleSystem.IntegrationTests._Environment.FluentMigration;
//using SampleSystem.IntegrationTests._Environment.TestData;

//namespace SampleSystem.IntegrationTests.__Support.Utils;

////public class SampleSystemTestDatabaseGenerator(
////    IDatabaseContext databaseContext,
////    IOptions<AutomationFrameworkSettings> settings,
////    TestDataInitializer testDataInitializer)
////    : TestDatabaseGenerator(databaseContext, settings)
////{
////    public override IEnumerable<string> TestServers => new List<string> { "." };

////    public async override Task GenerateDatabasesAsync() =>
////        new DbGeneratorTest().GenerateAllDB(
////            this.DatabaseContext.ConnectionString.DataSource,
////            mainDatabaseName: this.DatabaseContext.ConnectionString.DataSource,
////            credential: DbUserCredential.Create(
////                this.DatabaseContext.ConnectionString.UserId,
////                this.DatabaseContext.ConnectionString.Password));

////    public override async Task CheckTestDatabaseAsync()
////    {
////        if (this.DatabaseContext.Server.TableRowCount(this.DatabaseContext.ConnectionString.DataSource, "Location") > 100)
////        {
////            throw new Exception(
////                "Location row count more than 100. Please ensure that you run tests in Test Environment. If you want to run tests in the environment, please delete all Location rows (Location table) manually and rerun tests.");
////        }
////    }

////    public override async Task GenerateTestDataAsync() => await testDataInitializer.InitializeAsync(CancellationToken.None);

////    public override void ExecuteInsertsForDatabases()
////    {
////        base.ExecuteInsertsForDatabases();

////        CoreDatabaseUtil.ExecuteSqlFromFolder(this.DatabaseContext.ConnectionString.ConnectionString, @"__Support/Scripts/Authorization", this.DatabaseContext.ConnectionString.DataSource);
////        CoreDatabaseUtil.ExecuteSqlFromFolder(this.DatabaseContext.ConnectionString.ConnectionString,@"__Support/Scripts/Configuration", this.DatabaseContext.ConnectionString.DataSource);

////        CoreDatabaseUtil.ExecuteSqlFromFolder(this.DatabaseContext.ConnectionString.ConnectionString,@"__Support/Scripts/SampleSystem", this.DatabaseContext.ConnectionString.DataSource);

////        new BssFluentMigrator(this.DatabaseContext.ConnectionString.ConnectionString, typeof(InitNumberInDomainObjectEventMigration).Assembly).Migrate();
////    }
////}

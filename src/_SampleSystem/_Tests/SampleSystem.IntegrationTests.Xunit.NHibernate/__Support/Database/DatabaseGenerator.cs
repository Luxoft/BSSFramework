using Automation.Settings;
using Automation.Utils.DatabaseUtils;
using Automation.Utils.DatabaseUtils.Interfaces;

using Framework.DomainDriven.DBGenerator;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using SampleSystem.DbGenerate;
using SampleSystem.IntegrationTests.__Support.FluentMigration;
using SampleSystem.IntegrationTests.Xunit.NHibernate.__Support.TestData;

namespace SampleSystem.IntegrationTests.Xunit.NHibernate.__Support.Database;

public class DatabaseGenerator(
    IDatabaseContext databaseContext,
    IOptions<AutomationFrameworkSettings> settings,
    IConfiguration configuration)
    : TestDatabaseGenerator(databaseContext, settings)
{
    public override IEnumerable<string> TestServers => new List<string> { "." };

    public override async Task GenerateDatabasesAsync()
    {
        new DbGeneratorTest().GenerateAllDB(
            this.DatabaseContext.Main.DataSource,
            mainDatabaseName: this.DatabaseContext.Main.DatabaseName,
            credential: DbUserCredential.Create(
                this.DatabaseContext.Main.UserId,
                this.DatabaseContext.Main.Password));
    }

    public override async Task CheckTestDatabaseAsync()
    {
        if (this.DatabaseContext.Server.TableRowCount(this.DatabaseContext.Main.DatabaseName, "Location") > 100)
        {
            throw new Exception(
                "Location row count more than 100. Please ensure that you run tests in Test Environment. If you want to run tests in the environment, please delete all Location rows (Location table) manually and rerun tests.");
        }
    }

    public override async Task GenerateTestDataAsync() => await new TestDataInitializer(configuration, this.DatabaseContext)
                                                              .InitializeAsync(CancellationToken.None);
    public override void ExecuteInsertsForDatabases()
    {
        base.ExecuteInsertsForDatabases();

        CoreDatabaseUtil.ExecuteSqlFromFolder(this.DatabaseContext.Main.ConnectionString, @"__Support/Scripts/Authorization", this.DatabaseContext.Main.DatabaseName);
        CoreDatabaseUtil.ExecuteSqlFromFolder(this.DatabaseContext.Main.ConnectionString,@"__Support/Scripts/Configuration", this.DatabaseContext.Main.DatabaseName);
        CoreDatabaseUtil.ExecuteSqlFromFolder(this.DatabaseContext.Main.ConnectionString,@"__Support/Scripts/SampleSystem", this.DatabaseContext.Main.DatabaseName);

        new BssFluentMigrator(this.DatabaseContext.Main.ConnectionString, typeof(InitNumberInDomainObjectEventMigration).Assembly).Migrate();
    }
}

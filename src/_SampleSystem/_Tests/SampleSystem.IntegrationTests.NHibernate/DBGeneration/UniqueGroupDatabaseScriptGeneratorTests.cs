using Framework.AutomationCore.RootServiceProviderContainer;
using Framework.Database.NHibernate.DBGenerator;

using Microsoft.SqlServer.Management.Smo;

using SampleSystem.DbGenerate.NHibernate;
using SampleSystem.IntegrationTests._Environment.TestData;

using Index = Microsoft.SqlServer.Management.Smo.Index;

namespace SampleSystem.IntegrationTests.DBGeneration;

public class UniqueGroupDatabaseScriptGeneratorTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public void GenerateLocal_UniqueIndexExistsWithLessColumns_RecreatesColumns()
    {
        // Arrange
        var generator = new DbGeneratorTest();

        var tableName = "RoleRoleDegreeLink";

        var table = this.DataManager.GetTable(this.DatabaseContext.Main.DatabaseName, tableName);

        var indexName = "unilink_RoleRoleDegreeLink";

        var index = table.Indexes[indexName];
        index.Drop();

        var newIndex = new Index(table, indexName) { IndexKeyType = IndexKeyType.DriUniqueKey };
        var column = new IndexedColumn(newIndex, "roleDegreeId");
        newIndex.IndexedColumns.Add(column);

        newIndex.Create();

        // Act
        generator.GenerateAllDB(
            this.DatabaseContext.Main.DataSource,
            this.DatabaseContext.Main.DatabaseName,
            credential: DbUserCredential.Create(
                this.DatabaseContext.Main.UserId,
                this.DatabaseContext.Main.Password),
            skipFrameworkDatabases: true);

        var changedTable = this.DataManager.GetTable(this.DatabaseContext.Main.DatabaseName, tableName);

        //  changedTable.Indexes.Refresh();
        var indexes = changedTable.Indexes.ToList();

        // Assert
        Assert.Contains(indexes, x => x.Name == indexName);
        var indexedColumns = indexes.First(x => x.Name == indexName).IndexedColumns.ToList();
        Assert.Equal(2, indexedColumns.Count);
        Assert.Contains(indexedColumns, x => x.Name == "roleDegreeId");
        Assert.Contains(indexedColumns, x => x.Name == "roleId");
    }
}

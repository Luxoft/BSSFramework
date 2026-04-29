using Framework.AutomationCore.RootServiceProviderContainer;
using Framework.Database.NHibernate.DBGenerator;

using Microsoft.SqlServer.Management.Smo;

using SampleSystem.DbGenerate.NHibernate;
using SampleSystem.IntegrationTests._Environment.TestData;

using Index = Microsoft.SqlServer.Management.Smo.Index;

namespace SampleSystem.IntegrationTests.DBGeneration;

public class ChangeIndexesStrategyTests(IServiceProvider rootServiceProvider) : TestBase(rootServiceProvider)
{
    [Fact]
    public void GenerateLocal_ColumnHasIndexWithIncludedColumns_PreventsDefaultIndexFromGeneration()
    {
        // Arrange
        var generator = new DbGeneratorTest();

        var tableName = "Employee";
        var table = this.DataManager.GetTable(this.DatabaseContext.Main.DatabaseName, tableName);

        var baseIndexName = "IX_Employee_coreBusinessUnitId";
        var newIndexName = "IX_Employee_coreBusinessUnitId_inc";

        var index = table.Indexes[baseIndexName];
        index.Drop();

        var newIndex = new Index(table, newIndexName);
        var column = new IndexedColumn(newIndex, "coreBusinessUnitId");
        newIndex.IndexedColumns.Add(column);
        var includedColumn = new IndexedColumn(newIndex, "roleId")
                             {
                                     IsIncluded = true
                             };
        newIndex.IndexedColumns.Add(includedColumn);

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

        // Assert
        var indexes = changedTable.Indexes.ToList();
        Assert.Contains(indexes, x => x.Name == newIndexName);
        Assert.DoesNotContain(indexes, x => x.Name == baseIndexName);
    }

    [Fact]
    public void GenerateLocal_IgnoredIndex_NotCreated()
    {
        // Arrange
        var generator = new DbGeneratorTest();

        var tableName = "Employee";
        var table = this.DataManager.GetTable(this.DatabaseContext.Main.DatabaseName, tableName);

        var ignoredIndexName = "IX_Employee_hRDepartmentId";

        var index = table.Indexes[ignoredIndexName];
        index.Drop();

        // Act
        generator.GenerateAllDB(
                                this.DatabaseContext.Main.DataSource,
                                this.DatabaseContext.Main.DatabaseName,
                                credential: DbUserCredential.Create(
                                                                  this.DatabaseContext.Main.UserId,
                                                                  this.DatabaseContext.Main.Password),
                                ignoredIndexes: [ignoredIndexName],
                                skipFrameworkDatabases: true);
        var changedTable = this.DataManager.GetTable(this.DatabaseContext.Main.DatabaseName, tableName);

        // Assert
        Assert.DoesNotContain(changedTable.Indexes, x => x.Name == ignoredIndexName);
    }

    [Fact]
    public void GenerateLocal_UniqueFieldForFK_NoDuplicates()
    {
        // Arrange
        var generator = new DbGeneratorTest();

        // Act
        generator.GenerateAllDB(
                                this.DatabaseContext.Main.DataSource,
                                this.DatabaseContext.Main.DatabaseName,
                                credential: DbUserCredential.Create(
                                                                  this.DatabaseContext.Main.UserId,
                                                                  this.DatabaseContext.Main.Password),
                                skipFrameworkDatabases: true);

        var changedTable = this.DataManager.GetTable(this.DatabaseContext.Main.DatabaseName, "Employee");

        // Assert
        Assert.DoesNotContain(changedTable.Indexes, x => x.Name == "IX_ChildEntity_parentId");
    }
}

using System.Linq;

using FluentAssertions;

using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.DbGenerate;
using SampleSystem.IntegrationTests.__Support;
using Framework.DomainDriven.DBGenerator;

namespace SampleSystem.IntegrationTests.DBGeneration;

[TestClass]
public class ChangeIndexesStrategyTests : TestBase
{
    [TestMethod]
    public void GenerateLocal_ColumnHasIndexWithIncludedColumns_PreventsDefaultIndexFromGeneration()
    {
        // Arrange
        var generator = new DbGeneratorTest();

        var tableName = "Employee";
        var table = this.DataHelper.GetTable(this.DatabaseContext.Main.DatabaseName, tableName);

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
                                credential: UserCredential.Create(
                                                                  this.DatabaseContext.Main.UserId,
                                                                  this.DatabaseContext.Main.Password),
                                skipFrameworkDatabases: true);

        var changedTable = this.DataHelper.GetTable(this.DatabaseContext.Main.DatabaseName, tableName);

        // Assert
        changedTable.Indexes.Cast<Index>()
                    .Should()
                    .Contain(x => x.Name == newIndexName)
                    .And.NotContain(x => x.Name == baseIndexName);
    }

    [TestMethod]
    public void GenerateLocal_IgnoredIndex_NotCreated()
    {
        // Arrange
        var generator = new DbGeneratorTest();

        var tableName = "Employee";
        var table = this.DataHelper.GetTable(this.DatabaseContext.Main.DatabaseName, tableName);

        var ignoredIndexName = "IX_Employee_hRDepartmentId";

        var index = table.Indexes[ignoredIndexName];
        index.Drop();

        // Act
        generator.GenerateAllDB(
                                this.DatabaseContext.Main.DataSource,
                                this.DatabaseContext.Main.DatabaseName,
                                credential: UserCredential.Create(
                                                                  this.DatabaseContext.Main.UserId,
                                                                  this.DatabaseContext.Main.Password),
                                ignoredIndexes: new[] { ignoredIndexName },
                                skipFrameworkDatabases: true);
        var changedTable = this.DataHelper.GetTable(this.DatabaseContext.Main.DatabaseName, tableName);

        // Assert
        changedTable.Indexes.Cast<Index>()
                    .Should()
                    .NotContain(x => x.Name == ignoredIndexName);
    }

    [TestMethod]
    public void GenerateLocal_UniqueFieldForFK_NoDuplicates()
    {
        // Arrange
        var generator = new DbGeneratorTest();

        // Act
        generator.GenerateAllDB(
                                this.DatabaseContext.Main.DataSource,
                                this.DatabaseContext.Main.DatabaseName,
                                credential: UserCredential.Create(
                                                                  this.DatabaseContext.Main.UserId,
                                                                  this.DatabaseContext.Main.Password),
                                skipFrameworkDatabases: true);

        var changedTable = this.DataHelper.GetTable(this.DatabaseContext.Main.DatabaseName, "Employee");

        // Assert
        changedTable.Indexes.Cast<Index>()
                    .Should()
                    .NotContain(x => x.Name == "IX_ChildEntity_parentId");
    }
}

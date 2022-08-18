using System.Linq;

using FluentAssertions;

using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleSystem.IntegrationTests.__Support.TestData;
using SampleSystem.DbGenerate;
using SampleSystem.IntegrationTests.__Support;
using Framework.DomainDriven.DBGenerator;

namespace SampleSystem.IntegrationTests.DBGeneration
{
    [TestClass]
    public class UniqueGroupDatabaseScriptGeneratorTests : TestBase
    {
        [TestMethod]
        public void GenerateLocal_UniqueIndexExistsWithLessColumns_RecreatesColumns()
        {
            // Arrange
            var generator = new DbGeneratorTest();

            var tableName = "RoleRoleDegreeLink";

            var table = this.DataHelper.GetTable(this.DatabaseUtil.DatabaseContext.MainDatabase.DatabaseName, tableName);

            var indexName = "unilink_RoleRoleDegreeLink";

            var index = table.Indexes[indexName];
            index.Drop();

            var newIndex = new Index(table, indexName)
            {
                IndexKeyType = IndexKeyType.DriUniqueKey
            };
            var column = new IndexedColumn(newIndex, "roleDegreeId");
            newIndex.IndexedColumns.Add(column);

            newIndex.Create();

            // Act
            generator.GenerateAllDB(
                this.DatabaseUtil.DatabaseContext.MainDatabase.DataSource,
                credential: UserCredential.Create(
                    this.DatabaseUtil.DatabaseContext.MainDatabase.UserId,
                    this.DatabaseUtil.DatabaseContext.MainDatabase.Password),
                skipFrameworkDatabases: true);

            var changedTable = this.DataHelper.GetTable(this.DatabaseUtil.DatabaseContext.MainDatabase.DatabaseName, tableName);
            var indexes = changedTable.Indexes.Cast<Index>().ToList();

            // Assert
            indexes.Should().Contain(x => x.Name == indexName);
            indexes.First(x => x.Name == indexName)
                   .IndexedColumns.Cast<IndexedColumn>()
                   .Should()
                   .HaveCount(2)
                   .And.Contain(x => x.Name == "roleDegreeId")
                   .And.Contain(x => x.Name == "roleId");
        }
    }
}

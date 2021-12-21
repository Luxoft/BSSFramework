using System;
using System.Collections.Generic;
using System.Linq;

using Framework.DomainDriven.DBGenerator;
using Framework.DomainDriven.DBGenerator.ScriptGenerators.ScriptGeneratorStrategy;
using Framework.DomainDriven.Metadata;
using Framework.Persistent;

using Microsoft.SqlServer.Management.Smo;

using NUnit.Framework;

namespace DBGenerator.Tests.Unit
{
    [TestFixture]
    public class ChangeDefaultInitializedValueStrategyTest
    {
        [Test]
        public void Execute_NotAddedNewFields_OnlyTemplateScriptGenerated()
        {
            // Arrange
            var databaseScriptGeneratorStrategeInfo = this.CreateDatabaseScriptGeneratorStrategeInfo(Enumerable.Empty<DomainTypeMetadata>());

            var changeDefaultInitializedValueStrategy = new ChangeDefaultInitializedValueStrategy(databaseScriptGeneratorStrategeInfo);

            // Act
            var resultScript = changeDefaultInitializedValueStrategy.Execute()();

            // Assert
            var clippedScript = SkipDefaultTemplate(resultScript).ToList();
            Assert.AreEqual(0, clippedScript.Count);
        }

        [Test]
        public void Execute_AddedNewNotVersionField_OnlyTemplateScriptGenerated()
        {
            // Arrange
            var domainTypeMetadata = new DomainTypeMetadata(typeof(object), new AssemblyMetadata(typeof(object)));
            domainTypeMetadata.AddFields(new[] { new PrimitiveTypeFieldMetadata("test", typeof(string), Enumerable.Empty<Attribute>(), domainTypeMetadata, false) });

            var databaseScriptGeneratorStrategeInfo = this.CreateDatabaseScriptGeneratorStrategeInfo(new[] { domainTypeMetadata });

            var table = new Table { Name = "Object" };
            var column = new Column { Name = "test" };
            databaseScriptGeneratorStrategeInfo.AddedColumns.Add(new Tuple<Table, Column, string>(table, column, "0"));

            var changeDefaultInitializedValueStrategy = new ChangeDefaultInitializedValueStrategy(databaseScriptGeneratorStrategeInfo);

            // Act
            var resultScript = changeDefaultInitializedValueStrategy.Execute()();

            // Assert
            var clippedScript = SkipDefaultTemplate(resultScript).ToList();
            Assert.AreEqual(0, clippedScript.Count);
        }

        [Test]
        public void Execute_AddedNewVersionField_GeneratedUpdateValueScript()
        {
            // Arrange
            var domainTypeMetadata = new DomainTypeMetadata(typeof(object), new AssemblyMetadata(typeof(object)));
            domainTypeMetadata.AddFields(new[] { new PrimitiveTypeFieldMetadata("test", typeof(string), new[] { new VersionAttribute() }, domainTypeMetadata, false) });

            var databaseScriptGeneratorStrategeInfo = this.CreateDatabaseScriptGeneratorStrategeInfo(new[] { domainTypeMetadata });

            var table = new Table { Name = "Object" };
            var column = new Column { Name = "test" };
            databaseScriptGeneratorStrategeInfo.AddedColumns.Add(new Tuple<Table, Column, string>(table, column, "0"));

            var changeDefaultInitializedValueStrategy = new ChangeDefaultInitializedValueStrategy(databaseScriptGeneratorStrategeInfo);

            // Act
            var resultScript = changeDefaultInitializedValueStrategy.Execute()();

            // Assert
            var clippedScript = SkipDefaultTemplate(resultScript).ToList();
            Assert.AreEqual(1, clippedScript.Count);
            Assert.AreEqual("update Object set [test]=0\r\n", clippedScript.Single());
        }

        private DatabaseScriptGeneratorStrategyInfo CreateDatabaseScriptGeneratorStrategeInfo(IEnumerable<DomainTypeMetadata> domainTypeMetadata)
        {
            var scriptGeneratorContext = new DatabaseScriptGeneratorContextMockBuilder();
            var databaseScriptGeneratorContext = scriptGeneratorContext.DatabaseScriptGeneratorContext;

            var databaseScriptGeneratorStrategeInfo = new DatabaseScriptGeneratorStrategyInfo(
                databaseScriptGeneratorContext,
                domainTypeMetadata,
                DatabaseScriptGeneratorMode.None,
                new DataTypeComparer(),
                string.Empty,
                new List<string>());

            return databaseScriptGeneratorStrategeInfo;
        }

        private static IEnumerable<string> SkipDefaultTemplate(IEnumerable<string> script)
        {
            return script.Where(x => x != "GO" && !x.StartsWith("-------------") && x != "use []" + Environment.NewLine && x != Environment.NewLine);
        }
    }
}

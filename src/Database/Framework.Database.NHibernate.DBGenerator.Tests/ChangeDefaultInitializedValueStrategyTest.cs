using Framework.Database.Attributes;
using Framework.Database.Metadata;
using Framework.Database.NHibernate.DBGenerator.ScriptGenerators.ScriptGeneratorStrategy;
using Framework.Database.NHibernate.DBGenerator.Tests.Support;

using Microsoft.SqlServer.Management.Smo;

using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Framework.Database.NHibernate.DBGenerator.Tests;

[TestFixture]
public class ChangeDefaultInitializedValueStrategyTest
{
    [Test]
    public void Execute_NotAddedNewFields_OnlyTemplateScriptGenerated()
    {
        // Arrange
        var databaseScriptGeneratorStrategyInfo = this.CreateDatabaseScriptGeneratorStrategyInfo([]);

        var changeDefaultInitializedValueStrategy = new ChangeDefaultInitializedValueStrategy(databaseScriptGeneratorStrategyInfo);

        // Act
        var resultScript = changeDefaultInitializedValueStrategy.Execute()();

        // Assert
        var clippedScript = SkipDefaultTemplate(resultScript).ToList();
        ClassicAssert.AreEqual(0, clippedScript.Count);
    }

    [Test]
    public void Execute_AddedNewNotVersionField_OnlyTemplateScriptGenerated()
    {
        // Arrange
        var domainTypeMetadata = new DomainTypeMetadata(typeof(object), new AssemblyMetadata(typeof(object)));
        domainTypeMetadata.AddFields([new PrimitiveTypeFieldMetadata("test", typeof(string), [], domainTypeMetadata, false)]);

        var databaseScriptGeneratorStrategyInfo = this.CreateDatabaseScriptGeneratorStrategyInfo([domainTypeMetadata]);

        var table = new Table { Name = "Object" };
        var column = new Column { Name = "test" };
        databaseScriptGeneratorStrategyInfo.AddedColumns.Add(new Tuple<Table, Column, string>(table, column, "0"));

        var changeDefaultInitializedValueStrategy = new ChangeDefaultInitializedValueStrategy(databaseScriptGeneratorStrategyInfo);

        // Act
        var resultScript = changeDefaultInitializedValueStrategy.Execute()();

        // Assert
        var clippedScript = SkipDefaultTemplate(resultScript).ToList();
        ClassicAssert.AreEqual(0, clippedScript.Count);
    }

    [Test]
    public void Execute_AddedNewVersionField_GeneratedUpdateValueScript()
    {
        // Arrange
        var domainTypeMetadata = new DomainTypeMetadata(typeof(object), new AssemblyMetadata(typeof(object)));
        domainTypeMetadata.AddFields([new PrimitiveTypeFieldMetadata("test", typeof(string), [new VersionAttribute()], domainTypeMetadata, false)]);

        var databaseScriptGeneratorStrategyInfo = this.CreateDatabaseScriptGeneratorStrategyInfo([domainTypeMetadata]);

        var table = new Table { Name = "Object" };
        var column = new Column { Name = "test" };
        databaseScriptGeneratorStrategyInfo.AddedColumns.Add(new Tuple<Table, Column, string>(table, column, "0"));

        var changeDefaultInitializedValueStrategy = new ChangeDefaultInitializedValueStrategy(databaseScriptGeneratorStrategyInfo);

        // Act
        var resultScript = changeDefaultInitializedValueStrategy.Execute()();

        // Assert
        var clippedScript = SkipDefaultTemplate(resultScript).ToList();
        ClassicAssert.AreEqual(1, clippedScript.Count);
        ClassicAssert.AreEqual($"update Object set [test]=0{Environment.NewLine}", clippedScript.Single());
    }

    private DatabaseScriptGeneratorStrategyInfo CreateDatabaseScriptGeneratorStrategyInfo(IEnumerable<DomainTypeMetadata> domainTypeMetadata)
    {
        var scriptGeneratorContext = new DatabaseScriptGeneratorContextMockBuilder();
        var databaseScriptGeneratorContext = scriptGeneratorContext.DatabaseScriptGeneratorContext;

        var databaseScriptGeneratorStrategyInfo = new DatabaseScriptGeneratorStrategyInfo(
         databaseScriptGeneratorContext,
         domainTypeMetadata,
         DatabaseScriptGeneratorMode.None,
         new DataTypeComparer(),
         string.Empty,
         new List<string>());

        return databaseScriptGeneratorStrategyInfo;
    }

    private static IEnumerable<string> SkipDefaultTemplate(IEnumerable<string> script) => script.Where(x => x != "GO" && !x.StartsWith("-------------") && x != "use []" + Environment.NewLine && x != Environment.NewLine);
}

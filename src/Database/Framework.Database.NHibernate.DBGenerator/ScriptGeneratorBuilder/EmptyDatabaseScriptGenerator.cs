using Framework.Database.NHibernate.DBGenerator.Contracts;
using Framework.Database.NHibernate.DBGenerator.Team;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGeneratorBuilder;

class EmptyDatabaseScriptGenerator : IDatabaseScriptGenerator
{
    internal static EmptyDatabaseScriptGenerator Value = new();
    private EmptyDatabaseScriptGenerator()
    {

    }
    public IDatabaseScriptResult GenerateScript(IDatabaseScriptGeneratorContext context) => DatabaseScriptResultFactory.Create(new Dictionary<ApplyMigrationDbScriptMode, Lazy<IEnumerable<string>>>());
}

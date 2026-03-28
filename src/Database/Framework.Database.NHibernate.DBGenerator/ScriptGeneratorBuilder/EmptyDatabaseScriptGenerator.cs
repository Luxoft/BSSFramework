using Framework.Database.NHibernate.DBGenerator.Contracts;
using Framework.Database.NHibernate.DBGenerator.Team;

namespace Framework.Database.NHibernate.DBGenerator.ScriptGeneratorBuilder;

class EmptyDatabaseScriptGenerator : IDatabaseScriptGenerator
{
    internal static EmptyDatabaseScriptGenerator Value = new EmptyDatabaseScriptGenerator();
    private EmptyDatabaseScriptGenerator()
    {

    }
    public IDatabaseScriptResult GenerateScript(IDatabaseScriptGeneratorContext context)
    {
        return DatabaseScriptResultFactory.Create(new Dictionary<ApplyMigrationDbScriptMode, Lazy<IEnumerable<string>>>());
    }
}

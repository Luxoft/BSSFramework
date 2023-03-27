using Framework.DomainDriven.DBGenerator.Contracts;
using Framework.DomainDriven.DBGenerator.Team;

namespace Framework.DomainDriven.DBGenerator;

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

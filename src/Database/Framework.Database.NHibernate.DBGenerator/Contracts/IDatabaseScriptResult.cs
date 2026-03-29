using Framework.Database.NHibernate.DBGenerator.Team;

namespace Framework.Database.NHibernate.DBGenerator.Contracts;

public interface IDatabaseScriptResult
{
    IEnumerable<string> this[ApplyMigrationDbScriptMode mode] { get; }

    IEnumerable<IEnumerable<string>> GetResults();

    string ToNewLinesCombined();

    IDatabaseScriptResult Evaluate();
}

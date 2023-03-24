using System.Collections.Generic;

using Framework.DomainDriven.DBGenerator.Team;

namespace Framework.DomainDriven.DBGenerator.Contracts;

public interface IDatabaseScriptResult
{
    IEnumerable<string> this[ApplyMigrationDbScriptMode mode] { get; }

    IEnumerable<IEnumerable<string>> GetResults();

    string ToNewLinesCombined();

    IDatabaseScriptResult Evaluate();
}

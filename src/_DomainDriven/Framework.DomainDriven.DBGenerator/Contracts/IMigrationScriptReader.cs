using System.Collections.Generic;
using Framework.DomainDriven.DBGenerator.Team;

namespace Framework.DomainDriven.DBGenerator.Contracts;

public interface IMigrationScriptReader
{
    IEnumerable<MigrationDbScript> Read();
}

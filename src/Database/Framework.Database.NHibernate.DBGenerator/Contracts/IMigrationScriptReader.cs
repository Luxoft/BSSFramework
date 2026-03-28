using Framework.Database.NHibernate.DBGenerator.Team;

namespace Framework.Database.NHibernate.DBGenerator.Contracts;

public interface IMigrationScriptReader
{
    IEnumerable<MigrationDbScript> Read();
}

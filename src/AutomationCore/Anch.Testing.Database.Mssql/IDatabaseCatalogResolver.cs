using Anch.Testing.Database.ConnectionStringManagement;

namespace Anch.Testing.Database.Mssql;

public interface IDatabaseCatalogResolver
{
    IEnumerable<string> GetCatalogs(TestConnectionStringRole connectionStringRole);
}

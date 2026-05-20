using Anch.Testing.Database.ConnectionStringManagement;

namespace Framework.AutomationCore.Services;

public interface IDatabaseCatalogResolver
{
    IEnumerable<string> GetCatalogs(TestConnectionStringRole connectionStringRole);
}

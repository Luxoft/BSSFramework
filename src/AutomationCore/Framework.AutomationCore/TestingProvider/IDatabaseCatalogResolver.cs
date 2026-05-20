using Anch.Testing.Database.ConnectionStringManagement;

namespace Framework.AutomationCore.TestingProvider;

public interface IDatabaseCatalogResolver
{
    IEnumerable<string> GetCatalogs(TestConnectionStringRole connectionStringRole);
}

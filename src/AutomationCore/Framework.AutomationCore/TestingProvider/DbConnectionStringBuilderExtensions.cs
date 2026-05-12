using System.Data.Common;

namespace Framework.AutomationCore.TestingProvider;

public static class DbConnectionStringBuilderExtensions
{
    private const string InitialCatalogKey = "Initial Catalog";

    extension(DbConnectionStringBuilder builder)
    {
        public string InitialCatalog
        {
            get => builder[InitialCatalogKey].ToString()!;
            set => builder[InitialCatalogKey] = value;
        }
    }
}

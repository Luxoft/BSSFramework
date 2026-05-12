using System.Text.RegularExpressions;

namespace Framework.AutomationCore.TestingProvider;

public static class CoreDatabaseUtil
{
    private static readonly Regex InitialCatalogRegex = new("Initial Catalog=(\\w+);", RegexOptions.Compiled);

    public static string CutInitialCatalog(string inputConnectionString) =>
        InitialCatalogRegex.Replace(inputConnectionString, "");
}

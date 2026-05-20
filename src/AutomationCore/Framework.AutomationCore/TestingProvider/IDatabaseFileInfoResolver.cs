namespace Framework.AutomationCore.TestingProvider;

public interface IDatabaseFileInfoResolver
{
    DatabaseFileInfo Resolve(string initialCatalog);
}

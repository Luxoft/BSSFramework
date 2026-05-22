namespace Framework.AutomationCore.Services;

public interface IDatabaseFileInfoResolver
{
    DatabaseFileInfo Resolve(string initialCatalog);
}

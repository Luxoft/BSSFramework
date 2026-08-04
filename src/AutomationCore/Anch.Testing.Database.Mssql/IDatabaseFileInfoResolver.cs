namespace Anch.Testing.Database.Mssql;

public interface IDatabaseFileInfoResolver
{
    DatabaseFileInfo Resolve(string initialCatalog);
}

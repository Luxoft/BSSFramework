namespace Anch.Testing.Database.Mssql;

public class DatabaseFileInfoResolver(MssqlDatabaseSettings settings) : IDatabaseFileInfoResolver
{
    public DatabaseFileInfo Resolve(string initialCatalog)
    {
        var dbPath = Path.Combine(settings.BackupPath, $"{initialCatalog}.mdf");

        var logPath = Path.Combine(settings.BackupPath, $"{initialCatalog}_log.ldf");

        return new DatabaseFileInfo(dbPath, logPath);
    }
}

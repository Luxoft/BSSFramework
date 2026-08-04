using Anch.Core;
using Anch.Testing.Database.ConnectionStringManagement;

namespace Anch.Testing.Database.Mssql;

public class MssqlDatabaseManager(
    MssqlDatabaseSettings settings,
    INativeDatabaseManager nativeDatabaseManager,
    IDatabaseCatalogResolver databaseCatalogResolver) : IDatabaseManager
{
    public async ValueTask CreateEmpty(TestConnectionStringRole connectionStringRole, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (!string.IsNullOrEmpty(settings.BackupPath) && !Directory.Exists(settings.BackupPath))
        {
            Directory.CreateDirectory(settings.BackupPath);
        }

        foreach (var initialCatalog in databaseCatalogResolver.GetCatalogs(connectionStringRole))
        {
            await nativeDatabaseManager.CreateEmpty(initialCatalog, ct);
        }
    }

    public async ValueTask<bool> Exists(TestConnectionStringRole connectionStringRole, CancellationToken ct)
    {
        foreach (var initialCatalog in databaseCatalogResolver.GetCatalogs(connectionStringRole))
        {
            if (!await nativeDatabaseManager.Exists(initialCatalog, ct))
            {
                return false;
            }
        }

        return true;
    }

    public async ValueTask Remove(TestConnectionStringRole connectionStringRole, CancellationToken ct)
    {
        foreach (var initialCatalog in databaseCatalogResolver.GetCatalogs(connectionStringRole))
        {
            await nativeDatabaseManager.Remove(initialCatalog, ct);
        }
    }

    public async ValueTask Copy(TestConnectionStringRole source, TestConnectionStringRole target, CancellationToken ct)
    {
        foreach (var (sourceCatalog, targetCatalog) in databaseCatalogResolver.GetCatalogs(source).ZipStrong(
                     databaseCatalogResolver.GetCatalogs(target),
                     (s, t) => (s, t)))
        {
            await nativeDatabaseManager.Copy(sourceCatalog, targetCatalog, ct);
        }
    }

    public async ValueTask Move(TestConnectionStringRole source, TestConnectionStringRole target, CancellationToken ct)
    {
        foreach (var (sourceCatalog, targetCatalog) in databaseCatalogResolver.GetCatalogs(source).ZipStrong(
                     databaseCatalogResolver.GetCatalogs(target),
                     (s, t) => (s, t)))
        {
            await nativeDatabaseManager.Move(sourceCatalog, targetCatalog, ct);
        }
    }
}

using Anch.Core;
using Anch.Testing.Database.ConnectionStringManagement;

using Microsoft.Extensions.Options;

namespace Framework.AutomationCore.Services;

public class BssDatabaseManager(
    IOptions<AutomationFrameworkSettings> automationFrameworkSettingsOptions,
    INativeDatabaseManager nativeDatabaseManager,
    IDatabaseCatalogResolver databaseCatalogResolver) : IDatabaseManager
{
    public async ValueTask CreateEmpty(TestConnectionStringRole connectionStringRole, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (!string.IsNullOrEmpty(automationFrameworkSettingsOptions.Value.BackupPath)
            && !Directory.Exists(automationFrameworkSettingsOptions.Value.BackupPath))
        {
            Directory.CreateDirectory(automationFrameworkSettingsOptions.Value.BackupPath);
        }

        foreach (var initialCatalog in databaseCatalogResolver.GetCatalogs(connectionStringRole))
        {
            await nativeDatabaseManager.CreateEmpty(initialCatalog, ct);
        }
    }

    public ValueTask<bool> Exists(TestConnectionStringRole connectionStringRole, CancellationToken ct) =>
        databaseCatalogResolver.GetCatalogs(connectionStringRole).ToAsyncEnumerable().AllAsync(nativeDatabaseManager.Exists, ct);

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


using Anch.Core;
using Anch.Testing.Database.ConnectionStringManagement;

using Microsoft.Extensions.Options;

namespace Framework.AutomationCore.TestingProvider;

public class BssDatabaseManager(
    IOptions<AutomationFrameworkSettings> automationFrameworkSettingsOptions,
    INativeDatabaseManager nativeDatabaseManager,
    IDatabaseCatalogResolver databaseCatalogResolver) : IDatabaseManager
{
    public async ValueTask CreateEmpty(TestConnectionStringRole connectionStringRole, CancellationToken сancellationToken)
    {
        сancellationToken.ThrowIfCancellationRequested();

        if (!string.IsNullOrEmpty(automationFrameworkSettingsOptions.Value.BackupPath)
            && !Directory.Exists(automationFrameworkSettingsOptions.Value.BackupPath))
        {
            Directory.CreateDirectory(automationFrameworkSettingsOptions.Value.BackupPath);
        }

        foreach (var initialCatalog in databaseCatalogResolver.GetCatalogs(connectionStringRole))
        {
            await nativeDatabaseManager.CreateEmpty(initialCatalog, сancellationToken);
        }
    }

    public ValueTask<bool> Exists(TestConnectionStringRole connectionStringRole, CancellationToken сancellationToken) =>
        databaseCatalogResolver.GetCatalogs(connectionStringRole).ToAsyncEnumerable().AllAsync(nativeDatabaseManager.Exists, сancellationToken);

    public async ValueTask Remove(TestConnectionStringRole connectionStringRole, CancellationToken сancellationToken)
    {
        foreach (var initialCatalog in databaseCatalogResolver.GetCatalogs(connectionStringRole))
        {
            await nativeDatabaseManager.Remove(initialCatalog, сancellationToken);
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

using Framework.AutomationCore.Services;

namespace Framework.AutomationCore.ServerManagement;

public interface ISqlServer
{
    void Refresh();

    ISqlServerDatabase? TryGetDatabase(string initialCatalog);

    ValueTask CreateEmptyAsync(string initialCatalog, DatabaseFileInfo fileInfo, CancellationToken ct);

    ValueTask AttachDatabaseAsync(string initialCatalog, DatabaseFileInfo fileInfo, CancellationToken ct);

    ValueTask DetachDatabaseAsync(string initialCatalog, CancellationToken ct);
}

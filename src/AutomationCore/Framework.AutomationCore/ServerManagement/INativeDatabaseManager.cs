namespace Framework.AutomationCore.ServerManagement;

public interface INativeDatabaseManager
{
    ValueTask CreateEmpty(string initialCatalog, CancellationToken ct);

    ValueTask<bool> Exists(string initialCatalog, CancellationToken ct);

    ValueTask Remove(string initialCatalog, CancellationToken ct);

    ValueTask Copy(string sourceInitialCatalog, string targetInitialCatalog, CancellationToken ct);

    ValueTask Move(string sourceInitialCatalog, string targetInitialCatalog, CancellationToken ct);
}

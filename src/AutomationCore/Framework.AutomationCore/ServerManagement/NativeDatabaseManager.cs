using System.Collections.Concurrent;

using Anch.Core;
using Anch.Threading;

using Framework.AutomationCore.Services;

namespace Framework.AutomationCore.ServerManagement;

public class NativeDatabaseManager(
    ISqlServerFactory sqlServerFactory,
    IDatabaseFileInfoResolver databaseFileInfoResolver) : INativeDatabaseManager
{
    private readonly ConcurrentDictionary<string, (ISqlServer, DatabaseFileInfo, AsyncLocker)> serverCache = [];

    private async ValueTask<TResult> EvaluateAsync<TResult>(
        string initialCatalog,
        Func<ISqlServer, DatabaseFileInfo, ValueTask<TResult>> action,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var (sqlServer, fileInfo, locker) = this.serverCache.GetOrAdd(initialCatalog, _ => (sqlServerFactory.Create(), databaseFileInfoResolver.Resolve(initialCatalog), new AsyncLocker()));

        using (await locker.CreateScope(ct))
        {
            sqlServer.Refresh();

            if (sqlServer.TryGetDatabase(initialCatalog) is { } db)
            {
                db.Validate(fileInfo);

                await sqlServer.DetachDatabaseAsync(db.Name, ct);
            }

            return await action(sqlServer, fileInfo);
        }
    }

    private async ValueTask EvaluateAsync(string initialCatalog, Func<ISqlServer, DatabaseFileInfo, ValueTask> action, CancellationToken ct) =>
        await this.EvaluateAsync(initialCatalog, action.ToDefaultValueTask(), ct);

    public ValueTask CreateEmpty(string initialCatalog, CancellationToken ct) =>
        this.EvaluateAsync(initialCatalog, (sqlServer, fileInfo) => sqlServer.CreateEmptyAsync(initialCatalog, fileInfo, ct), ct);

    public ValueTask<bool> Exists(string initialCatalog, CancellationToken ct) =>
        this.EvaluateAsync(initialCatalog, async (_, fileInfo) => fileInfo.IsExists(), ct);

    public ValueTask Remove(string initialCatalog, CancellationToken ct) =>
        this.EvaluateAsync(
            initialCatalog,
            async (_, fileInfo) =>
            {
                if (File.Exists(fileInfo.DbPath))
                {
                    File.Delete(fileInfo.DbPath);
                }

                if (File.Exists(fileInfo.LogPath))
                {
                    File.Delete(fileInfo.LogPath);
                }
            },
            ct);

    public ValueTask Copy(string sourceInitialCatalog, string targetInitialCatalog, CancellationToken ct) =>
        this.CopyMove(sourceInitialCatalog, targetInitialCatalog, false, ct);

    public ValueTask Move(string sourceInitialCatalog, string targetInitialCatalog, CancellationToken ct) =>
        this.CopyMove(sourceInitialCatalog, targetInitialCatalog, true, ct);

    private ValueTask CopyMove(string sourceInitialCatalog, string targetInitialCatalog, bool move, CancellationToken ct) =>
        this.EvaluateAsync(
            sourceInitialCatalog,
            (_, sourceFileInfo) =>
                this.EvaluateAsync(
                    targetInitialCatalog,
                    async (targetSqlServer, targetFileInfo) =>
                    {
                        if (move)
                        {
                            File.Move(sourceFileInfo.DbPath, targetFileInfo.DbPath, true);
                            File.Move(sourceFileInfo.LogPath, targetFileInfo.LogPath, true);
                        }
                        else
                        {
                            File.Copy(sourceFileInfo.DbPath, targetFileInfo.DbPath, true);
                            File.Copy(sourceFileInfo.LogPath, targetFileInfo.LogPath, true);
                        }

                        await targetSqlServer.AttachDatabaseAsync(targetInitialCatalog, targetFileInfo, ct);
                    },
                    ct),
            ct);
}

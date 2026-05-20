using System.Collections.Concurrent;
using System.Collections.Specialized;

using Anch.Threading;

using Framework.AutomationCore.Extensions;

using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.Services;

public class NativeDatabaseManager(
    ISqlServerFactory sqlServerFactory,
    IOptions<AutomationFrameworkSettings> automationFrameworkSettingsOptions,
    IDatabaseFileInfoResolver databaseFileInfoResolver) : INativeDatabaseManager
{
    private readonly ConcurrentDictionary<string, (Server, AsyncLocker)> serverCache = [];

    private async Task<(Server, AsyncLocker)> GetServerAsync(string initialCatalog, CancellationToken ct)
    {
        var (server, locker) = this.serverCache.GetOrAdd(initialCatalog, _ => (sqlServerFactory.Create(), new AsyncLocker()));

        using (await locker.CreateScope(ct))
        {
            server.Refresh();
        }

        return (server, locker);
    }

    public async ValueTask CreateEmpty(string initialCatalog, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var fileInfo = databaseFileInfoResolver.Resolve(initialCatalog);
        var (sqlServer, _) = await this.GetServerAsync(initialCatalog, ct);

        var db = new Microsoft.SqlServer.Management.Smo.Database(sqlServer, initialCatalog);

        if (!string.IsNullOrEmpty(automationFrameworkSettingsOptions.Value.DatabaseCollation))
        {
            db.Collation = automationFrameworkSettingsOptions.Value.DatabaseCollation;
        }

        var fileGroup = new FileGroup(db, "PRIMARY");

        var dataFile = new DataFile(fileGroup, Path.GetFileNameWithoutExtension(fileInfo.DbPath), fileInfo.DbPath)
                       {
                           Size = 5.5 * 1024.0, GrowthType = FileGrowthType.KB, Growth = 1.0 * 1024.0
                       };

        fileGroup.Files.Add(dataFile);

        db.FileGroups.Add(fileGroup);

        var logFile = new LogFile(db, Path.GetFileNameWithoutExtension(fileInfo.LogPath), fileInfo.LogPath)
                      {
                          Size = 1.0 * 1024.0, GrowthType = FileGrowthType.Percent, Growth = 10.0
                      };

        db.LogFiles.Add(logFile);

        db.Create();
    }

    public async ValueTask<bool> Exists(string initialCatalog, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var fileInfo = databaseFileInfoResolver.Resolve(initialCatalog);
        var (sqlServer, _) = await this.GetServerAsync(initialCatalog, ct);

        if (sqlServer.Databases[initialCatalog] is { } db)
        {
            db.Validate(fileInfo);

            return true;
        }
        else
        {
            return fileInfo.IsExists;
        }
    }

    public async ValueTask Remove(string initialCatalog, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var fileInfo = databaseFileInfoResolver.Resolve(initialCatalog);
        var (sqlServer, _) = await this.GetServerAsync(initialCatalog, ct);

        if (sqlServer.Databases[initialCatalog] is { } db)
        {
            db.Validate(fileInfo);

            await sqlServer.DetachDatabaseAsync(db.Name, ct);
        }

        if (File.Exists(fileInfo.DbPath))
        {
            File.Delete(fileInfo.DbPath);
        }

        if (File.Exists(fileInfo.LogPath))
        {
            File.Delete(fileInfo.LogPath);
        }
    }

    public ValueTask Copy(string sourceInitialCatalog, string targetInitialCatalog, CancellationToken ct) =>
        this.CopyMove(sourceInitialCatalog, targetInitialCatalog, false, ct);

    public ValueTask Move(string sourceInitialCatalog, string targetInitialCatalog, CancellationToken ct) =>
        this.CopyMove(sourceInitialCatalog, targetInitialCatalog, true, ct);

    private async ValueTask CopyMove(string sourceInitialCatalog, string targetInitialCatalog, bool move, CancellationToken ct)
    {
        var sourceFileInfo = databaseFileInfoResolver.Resolve(sourceInitialCatalog);
        var targetFileInfo = databaseFileInfoResolver.Resolve(targetInitialCatalog);
        var (targetSqlServer, _) = await this.GetServerAsync(targetInitialCatalog, ct);

        if (targetSqlServer.Databases[sourceInitialCatalog] is not null)
        {
            var (sourceSqlServer, sourceSqlServerLocker) = await this.GetServerAsync(sourceInitialCatalog, ct);

            using (await sourceSqlServerLocker.CreateScope(ct))
            {
                sourceSqlServer.Refresh();

                if (sourceSqlServer.Databases[sourceInitialCatalog] is { } sourceDb)
                {
                    sourceDb.Validate(sourceFileInfo);

                    await sourceSqlServer.DetachDatabaseAsync(sourceInitialCatalog, ct);
                }
            }
        }

        if (!sourceFileInfo.IsExists)
        {
            throw new InvalidOperationException("Source database not found.");
        }

        if (targetSqlServer.Databases[targetInitialCatalog] is { } targetDb)
        {
            targetDb.Validate(targetFileInfo);

            await targetSqlServer.DetachDatabaseAsync(targetDb.Name, ct);
        }

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

        targetSqlServer.AttachDatabase(targetInitialCatalog, new StringCollection { targetFileInfo.DbPath, targetFileInfo.LogPath });
    }
}

using System.Collections.Concurrent;
using System.Collections.Specialized;

using Framework.AutomationCore.Services.DatabaseUtils;

using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.TestingProvider;

public class NativeDatabaseManager(
    ISqlServerFactory sqlServerFactory,
    IOptions<AutomationFrameworkSettings> automationFrameworkSettingsOptions,
    IDatabaseFileInfoResolver databaseFileInfoResolver) : INativeDatabaseManager
{
    private readonly ConcurrentDictionary<string, Server> serverCache = [];

    private Server GetServer(string initialCatalog)
    {
        var server = this.serverCache.GetOrAdd(initialCatalog, _ => sqlServerFactory.Create());

        server.Refresh();

        return server;
    }

    public ValueTask CreateEmpty(string initialCatalog, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var fileInfo = databaseFileInfoResolver.Resolve(initialCatalog);
        var sqlServer = this.GetServer(initialCatalog);

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

        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> Exists(string initialCatalog, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var fileInfo = databaseFileInfoResolver.Resolve(initialCatalog);
        var sqlServer = this.GetServer(initialCatalog);

        if (sqlServer.Databases[initialCatalog] is { } db)
        {
            db.Validate(fileInfo);

            return new(true);
        }
        else
        {
            return new(fileInfo.IsExists);
        }
    }

    public ValueTask Remove(string initialCatalog, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var fileInfo = databaseFileInfoResolver.Resolve(initialCatalog);
        var sqlServer = this.GetServer(initialCatalog);

        if (sqlServer.Databases[initialCatalog] is { } db)
        {
            db.Validate(fileInfo);

            sqlServer.DetachDatabase(db.Name);
        }

        if (File.Exists(fileInfo.DbPath))
        {
            File.Delete(fileInfo.DbPath);
        }

        if (File.Exists(fileInfo.LogPath))
        {
            File.Delete(fileInfo.LogPath);
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask Copy(string sourceInitialCatalog, string targetInitialCatalog, CancellationToken ct) =>
        this.CopyMove(sourceInitialCatalog, targetInitialCatalog, false, ct);

    public ValueTask Move(string sourceInitialCatalog, string targetInitialCatalog, CancellationToken ct) =>
        this.CopyMove(sourceInitialCatalog, targetInitialCatalog, true, ct);

    private ValueTask CopyMove(
        string sourceInitialCatalog,
        string targetInitialCatalog,
        bool move,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var sourceFileInfo = databaseFileInfoResolver.Resolve(sourceInitialCatalog);
        var targetFileInfo = databaseFileInfoResolver.Resolve(targetInitialCatalog);
        var sourceSqlServer = this.GetServer(sourceInitialCatalog);
        var targetSqlServer = this.GetServer(targetInitialCatalog);

        if (sourceSqlServer.Databases[sourceInitialCatalog] is { } sourceDb)
        {
            sourceDb.Validate(sourceFileInfo);

            sourceSqlServer.DetachDatabase(sourceInitialCatalog);
        }
        else if (!sourceFileInfo.IsExists)
        {
            throw new InvalidOperationException("Source database not found.");
        }

        if (targetSqlServer.Databases[targetInitialCatalog] is { } targetDb)
        {
            targetDb.Validate(targetFileInfo);

            targetSqlServer.DetachDatabase(targetDb.Name);
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

        return ValueTask.CompletedTask;
    }

}

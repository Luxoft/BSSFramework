using System.Collections.Concurrent;
using System.Collections.Specialized;

using Anch.Core;
using Anch.Testing;
using Anch.Threading;

using Framework.AutomationCore.Extensions;

using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.Services;

public class NativeDatabaseManager(
    IServiceProvider sp,
    ITestEnvironment testEnvironment,
    ServiceProviderIndex serviceProviderIndex,
    ISqlServerFactory sqlServerFactory,
    IOptions<AutomationFrameworkSettings> automationFrameworkSettingsOptions,
    IDatabaseFileInfoResolver databaseFileInfoResolver,
    IServiceProviderPool serviceProviderPool) : INativeDatabaseManager
{
    private readonly ConcurrentDictionary<string, (Server, DatabaseFileInfo, AsyncLocker)> serverCache = [];

    private async ValueTask<TResult> EvaluateAsync<TResult>(
        string initialCatalog,
        Func<Server, DatabaseFileInfo, ValueTask<TResult>> action,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var (sqlServer, fileInfo, locker) = this.serverCache.GetOrAdd(initialCatalog, _ => (sqlServerFactory.Create(), databaseFileInfoResolver.Resolve(initialCatalog), new AsyncLocker()));

        try
        {
            using (await locker.CreateScope(ct))
            {
                sqlServer.Refresh();

                if (sqlServer.Databases[initialCatalog] is { } db)
                {
                    db.Validate(fileInfo);

                    await sqlServer.DetachDatabaseAsync(db.Name, ct);
                }

                return await action(sqlServer, fileInfo);
            }
        }
        catch (Exception ex)
        {
            var sourceDir = Path.GetDirectoryName(fileInfo.DbPath)!;
            var filesInDir = Directory.Exists(sourceDir)
                                 ? Directory.GetFiles(sourceDir)
                                 : [];

            throw new IOException(
                $"{nameof(fileInfo.DbPath)}: {fileInfo.DbPath}\n" +
                $"{nameof(Environment.ProcessorCount)}: {Environment.ProcessorCount}\n" +
                $"Source directory ({sourceDir}) files (Count: {filesInDir.Length}):\n{string.Join("\n", filesInDir)}\n" +
                $"HashCode: {this.GetHashCode()}\n" +
                $"{nameof(Environment.ProcessId)}: {Environment.ProcessId}\n" +
                $"{nameof(serviceProviderIndex)}: {serviceProviderIndex.Index}\n" +
                $"{nameof(IServiceProvider)} HashCode: {sp.GetHashCode()}\n" +
                $"{nameof(ITestEnvironment)} HashCode: {testEnvironment.GetHashCode()}\n" +
                $"{nameof(ITestEnvironment)} {nameof(BssTestEnvironment.DebugIndex)}: {BssTestEnvironment.DebugIndex}\n" +
                $"{nameof(IServiceProviderPool)} {nameof(IServiceProviderPool.IsRoot)}: {serviceProviderPool.IsRoot}\n" +
                $"{nameof(IServiceProviderPool)} HashCode: {serviceProviderPool.GetHashCode()}\n" +
                $"{nameof(IServiceProviderPool.Inner)} {nameof(IServiceProviderPool)} HashCode: {serviceProviderPool.Inner?.GetHashCode()}\n" +
                $"{nameof(IServiceProviderPool.TestFramework)} HashCode: {serviceProviderPool.TestFramework.GetHashCode()}\n" +
                $"{nameof(IServiceProviderPool)} {nameof(IServiceProviderPool.MainIndex)}: {serviceProviderPool.MainIndex}\n" +
                $"{nameof(IServiceProviderPool)} {nameof(IServiceProviderPool.GlobalMainIndex)}: {serviceProviderPool.GlobalMainIndex}\n" +
                $"{nameof(IServiceProviderPool)} {nameof(IServiceProviderPool.AsyncLocker)} HashCode: {serviceProviderPool.AsyncLocker.GetHashCode()}\n",

                ex);
        }
    }

    private async ValueTask EvaluateAsync(string initialCatalog, Func<Server, DatabaseFileInfo, ValueTask> action, CancellationToken ct) =>
        await this.EvaluateAsync(initialCatalog, action.ToDefaultValueTask(), ct);

    public ValueTask CreateEmpty(string initialCatalog, CancellationToken ct) =>

        this.EvaluateAsync(
            initialCatalog,
            async (sqlServer, fileInfo) =>
            {
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
            },
            ct);

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

                        targetSqlServer.AttachDatabase(targetInitialCatalog, new StringCollection { targetFileInfo.DbPath, targetFileInfo.LogPath });
                    },
                    ct),
            ct);
}

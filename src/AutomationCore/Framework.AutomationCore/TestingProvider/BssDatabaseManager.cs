using System.Collections.Specialized;

using Anch.Testing.Database.ConnectionStringManagement;

using Framework.AutomationCore.Services.DatabaseUtils;

using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.TestingProvider;

public class BssDatabaseManager(
    IServerSource serverSource,
    IOptions<AutomationFrameworkSettings> automationFrameworkSettingsOptions,
    IDatabaseFileInfoExtractor fileInfoExtractor) : IDatabaseManager
{
    public ValueTask CreateEmpty(TestConnectionString connectionString, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (!string.IsNullOrEmpty(automationFrameworkSettingsOptions.Value.BackupPath)
            && !Directory.Exists(automationFrameworkSettingsOptions.Value.BackupPath))
        {
            Directory.CreateDirectory(automationFrameworkSettingsOptions.Value.BackupPath);
        }

        var fileInfo = fileInfoExtractor.Extract(connectionString);

        var db = new Microsoft.SqlServer.Management.Smo.Database(serverSource.Server, connectionString.InitialCatalog);

        if (!string.IsNullOrEmpty(automationFrameworkSettingsOptions.Value.DatabaseCollation))
        {
            db.Collation = automationFrameworkSettingsOptions.Value.DatabaseCollation;
        }

        var fileGroup = new FileGroup(db, "PRIMARY");

        var dataFile = new DataFile(fileGroup, connectionString.InitialCatalog, fileInfo.DbPath)
                       {
                           Size = 5.5 * 1024.0, GrowthType = FileGrowthType.KB, Growth = 1.0 * 1024.0
                       };

        fileGroup.Files.Add(dataFile);

        db.FileGroups.Add(fileGroup);

        var logFile = new LogFile(db, connectionString.InitialCatalog + "_log", fileInfo.LogPath)
                      {
                          Size = 1.0 * 1024.0, GrowthType = FileGrowthType.Percent, Growth = 10.0
                      };

        db.LogFiles.Add(logFile);

        db.Create();

        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> Exists(TestConnectionString connectionString, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var fileInfo = fileInfoExtractor.Extract(connectionString);

        if (serverSource.Server.GetDatabase(connectionString.InitialCatalog) is { } db)
        {
            db.Validate(fileInfo);

            return new(true);
        }
        else
        {
            return new(fileInfo.IsExists);
        }
    }

    public ValueTask Remove(TestConnectionString connectionString, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var fileInfo = fileInfoExtractor.Extract(connectionString);

        if (serverSource.Server.GetDatabase(connectionString.InitialCatalog) is { } db)
        {
            db.Validate(fileInfo);

            serverSource.Server.DetachDatabase(db.Name);
            //db.Drop();
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

    public ValueTask Copy(TestConnectionString from, TestConnectionString to, bool force, CancellationToken ct) =>
        this.CopyMove(from, to, force, move: false, ct);

    public ValueTask Move(TestConnectionString from, TestConnectionString to, bool force, CancellationToken ct) =>
        this.CopyMove(from, to, force, move: true, ct);

    private ValueTask CopyMove(
        TestConnectionString from,
        TestConnectionString to,
        bool force,
        bool move,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var sourceFileInfo = fileInfoExtractor.Extract(from);
        var targetFileInfo = fileInfoExtractor.Extract(to);

        if (serverSource.Server.GetDatabase(from.InitialCatalog) is { } sourceDb)
        {
            sourceDb.Validate(sourceFileInfo);

            serverSource.Server.DetachDatabase(from.InitialCatalog);
        }
        else if (!sourceFileInfo.IsExists)
        {
            throw new InvalidOperationException("Source database not found.");
        }

        if (serverSource.Server.GetDatabase(to.InitialCatalog) is { } targetDb)
        {
            targetDb.Validate(targetFileInfo);

            if (force)
            {
                serverSource.Server.DetachDatabase(targetDb.Name);
                //targetDb.Drop();
            }
            else
            {
                throw new InvalidOperationException("Target database already exists.");
            }
        }

        if (move)
        {
            File.Move(sourceFileInfo.DbPath, targetFileInfo.DbPath, overwrite: force);
            File.Move(sourceFileInfo.LogPath, targetFileInfo.LogPath, overwrite: force);
        }
        else
        {
            File.Copy(sourceFileInfo.DbPath, targetFileInfo.DbPath, overwrite: force);
            File.Copy(sourceFileInfo.LogPath, targetFileInfo.LogPath, overwrite: force);
        }

        serverSource.Server.AttachDatabase(to.InitialCatalog, new StringCollection { targetFileInfo.DbPath, targetFileInfo.LogPath });

        return ValueTask.CompletedTask;
    }
}

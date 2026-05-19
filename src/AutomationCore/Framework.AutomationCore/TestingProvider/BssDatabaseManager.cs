using System.Collections.Specialized;
using Anch.Testing.Database.ConnectionStringManagement;

using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.TestingProvider;

public class BssDatabaseManager(
    IMsSqlServerSource sqlServerSource,
    AutomationFrameworkSettings automationFrameworkSettings,
    IDatabaseFileInfoExtractor fileInfoExtractor) : IDatabaseManager
{
    public ValueTask CreateEmpty(TestConnectionString connectionString, CancellationToken ct)
    {
        var db = new Microsoft.SqlServer.Management.Smo.Database(sqlServerSource.Server, connectionString.InitialCatalog);

        if (!string.IsNullOrEmpty(automationFrameworkSettings.DatabaseCollation))
        {
            db.Collation = automationFrameworkSettings.DatabaseCollation;
        }

        CheckDirectoryAndCreateIfNotExists(automationFrameworkSettings.BackupPath);

        var fileGroup = new FileGroup(db, "PRIMARY");

        var fileInfo = fileInfoExtractor.Extract(connectionString);

        var dataFile = new DataFile(fileGroup, connectionString.InitialCatalog, fileInfo.DbPath)
                       {
                           Size = 5.5 * 1024.0,
                           GrowthType = FileGrowthType.KB,
                           Growth = 1.0 * 1024.0
                       };

        fileGroup.Files.Add(dataFile);

        db.FileGroups.Add(fileGroup);

        var logFile = new LogFile(db, connectionString.InitialCatalog + "_log", fileInfo.LogPath)
                      {
                          Size = 1.0 * 1024.0,
                          GrowthType = FileGrowthType.Percent,
                          Growth = 10.0
                      };

        db.LogFiles.Add(logFile);

        db.Create();

        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> Exists(TestConnectionString connectionString, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var fileInfo = fileInfoExtractor.Extract(connectionString);

        return new(File.Exists(fileInfo.DbPath) && new FileInfo(fileInfo.DbPath).Length > 0);
    }

    public ValueTask Remove(TestConnectionString connectionString, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var fileInfo = fileInfoExtractor.Extract(connectionString);

        if (File.Exists(fileInfo.DbPath))
        {
            sqlServerSource.Server.DetachDatabase(connectionString.InitialCatalog, false);

            File.Delete(fileInfo.DbPath);
            File.Delete(fileInfo.LogPath);
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask Copy(
        TestConnectionString from,
        TestConnectionString to,
        bool force,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var sourceFileInfo = fileInfoExtractor.Extract(from);
        var destinationFileInfo = fileInfoExtractor.Extract(to);

        if (!File.Exists(sourceFileInfo.DbPath))
        {
            throw new FileNotFoundException("Source database file not found.", sourceFileInfo.DbPath);
        }

        var destinationExists = File.Exists(destinationFileInfo.DbPath);

        if (destinationExists && !force)
        {
            throw new IOException($"Destination file already exists: {destinationFileInfo.DbPath}");
        }

        var directory = Path.GetDirectoryName(destinationFileInfo.DbPath);

        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (destinationExists)
        {
            sqlServerSource.Server.DetachDatabase(to.InitialCatalog, false);
        }

        File.Copy(sourceFileInfo.DbPath, destinationFileInfo.DbPath, overwrite: force);
        File.Copy(sourceFileInfo.LogPath, destinationFileInfo.LogPath, overwrite: force);

        sqlServerSource.Server.AttachDatabase(to.InitialCatalog, new StringCollection { destinationFileInfo.DbPath, destinationFileInfo.LogPath });

        return ValueTask.CompletedTask;
    }

    public ValueTask Move(
        TestConnectionString from,
        TestConnectionString to,
        bool force,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var sourceFileInfo = fileInfoExtractor.Extract(from);
        var destinationFileInfo = fileInfoExtractor.Extract(to);

        if (!File.Exists(sourceFileInfo.DbPath))
        {
            throw new FileNotFoundException("Source database file not found.", sourceFileInfo.DbPath);
        }

        var destinationExists = File.Exists(destinationFileInfo.DbPath);

        if (destinationExists)
        {
            if (!force)
            {
                throw new IOException($"Destination file already exists: {destinationFileInfo.DbPath}");
            }

            File.Delete(destinationFileInfo.DbPath);
            File.Delete(destinationFileInfo.LogPath);
        }

        var directory = Path.GetDirectoryName(destinationFileInfo.DbPath);

        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.Move(sourceFileInfo.DbPath, destinationFileInfo.DbPath);
        File.Move(sourceFileInfo.LogPath, destinationFileInfo.LogPath);

        return ValueTask.CompletedTask;
    }

    private static void CheckDirectoryAndCreateIfNotExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path!);
        }
    }
}

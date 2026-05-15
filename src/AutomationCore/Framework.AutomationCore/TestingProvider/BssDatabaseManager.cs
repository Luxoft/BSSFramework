using System.Collections.Specialized;
using Anch.Testing.Database.ConnectionStringManagement;

using Framework.AutomationCore.Services.DatabaseUtils;

namespace Framework.AutomationCore.TestingProvider;

public class BssDatabaseManager(IMsSqlServerSource serverSource, IDatabaseFilePathExtractor pathExtractor) : IDatabaseManager
{
    public ValueTask<bool> Exists(TestConnectionString connectionString, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var filePath = pathExtractor.Extract(connectionString);

        return new(File.Exists(filePath) && new FileInfo(filePath).Length > 0);
    }

    public ValueTask Remove(TestConnectionString connectionString, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var filePath = pathExtractor.Extract(connectionString);

        if (File.Exists(filePath))
        {
            serverSource.Server.DetachDatabase(connectionString.InitialCatalog);

            File.Delete(filePath);
            File.Delete(GetLogFile(filePath));
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

        var sourcePath = pathExtractor.Extract(from);
        var destinationPath = pathExtractor.Extract(to);

        if (!File.Exists(sourcePath))
        {
            throw new FileNotFoundException("Source database file not found.", sourcePath);
        }

        var destinationExists = File.Exists(destinationPath);

        if (destinationExists && !force)
        {
            throw new IOException($"Destination file already exists: {destinationPath}");
        }

        var directory = Path.GetDirectoryName(destinationPath);

        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (destinationExists)
        {
            serverSource.Server.DetachDatabase(to.InitialCatalog);
        }

        File.Copy(sourcePath, destinationPath, overwrite: force);
        File.Copy(GetLogFile(sourcePath), GetLogFile(destinationPath), overwrite: force);

        serverSource.Server.AttachDatabase(to.InitialCatalog, new StringCollection { destinationPath, GetLogFile(destinationPath) });

        return ValueTask.CompletedTask;
    }

    public ValueTask Move(
        TestConnectionString from,
        TestConnectionString to,
        bool force,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        var sourcePath = pathExtractor.Extract(from);
        var destinationPath = pathExtractor.Extract(to);

        if (!File.Exists(sourcePath))
        {
            throw new FileNotFoundException("Source database file not found.", sourcePath);
        }

        var destinationExists = File.Exists(destinationPath);

        if (destinationExists)
        {
            if (!force)
            {
                throw new IOException($"Destination file already exists: {destinationPath}");
            }

            File.Delete(destinationPath);
            File.Delete(GetLogFile(destinationPath));
        }

        var directory = Path.GetDirectoryName(destinationPath);

        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.Move(sourcePath, destinationPath);
        File.Move(GetLogFile(sourcePath), GetLogFile(destinationPath));

        return ValueTask.CompletedTask;
    }

    private static string GetLogFile(string mdfFilePath)
    {
        var directory = Path.GetDirectoryName(mdfFilePath);
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(mdfFilePath);
        return Path.Combine(directory ?? string.Empty, $"{fileNameWithoutExtension}_log.ldf");
    }
}

using System.Collections.Specialized;

using Framework.AutomationCore.Extensions;
using Framework.AutomationCore.Services;

using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.ServerManagement.LocalDb;

public class LocalDbSqlServer(Server server, IOptions<AutomationFrameworkSettings> automationFrameworkSettingsOptions) : ISqlServer
{
    public void Refresh() => server.Refresh();

    public ISqlServerDatabase? TryGetDatabase(string initialCatalog) =>
        server.Databases[initialCatalog] is { } db ? new LocalDbSqlServerDatabase(db) : null;

    public async ValueTask CreateEmptyAsync(string initialCatalog, DatabaseFileInfo fileInfo, CancellationToken ct)
    {
        var db = new Microsoft.SqlServer.Management.Smo.Database(server, initialCatalog);

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

    public async ValueTask AttachDatabaseAsync(string initialCatalog, DatabaseFileInfo fileInfo, CancellationToken ct) =>
        server.AttachDatabase(initialCatalog, new StringCollection { fileInfo.DbPath, fileInfo.LogPath });

    public async ValueTask DetachDatabaseAsync(string initialCatalog, CancellationToken ct) =>
        await server.DetachDatabaseAsync(initialCatalog, ct);
}

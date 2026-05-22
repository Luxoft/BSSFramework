using Framework.AutomationCore.Services;

using MartinCostello.SqlLocalDb;
using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.ServerManagement.LocalDb;

public class LocalDbSqlServerDatabase(ISqlLocalDbInstanceInfo sqlLocalDbInstanceInfo) : ISqlServerDatabase
{
    public string Name => sqlLocalDbInstanceInfo.Name;

    public void Validate(DatabaseFileInfo fileInfo)
    {
        if (!database.FileGroups.SelectMany(fg => fg.Files).Select(f => f.FileName).Contains(fileInfo.DbPath, StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Database primary file path '{database.PrimaryFilePath}' does not match expected path '{fileInfo.DbPath}'.");
        }

        if (!database.LogFiles.Select(lf => lf.FileName).Contains(fileInfo.LogPath, StringComparer.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException($"Database log file path '{fileInfo.LogPath}' does not match any existing log file paths.");
        }
    }
}

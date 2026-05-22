using Framework.AutomationCore.Services;

namespace Framework.AutomationCore.ServerManagement.Default;

public class SqlServerDatabase(Microsoft.SqlServer.Management.Smo.Database database) : ISqlServerDatabase
{
    public string Name => database.Name;

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

namespace Framework.AutomationCore.TestingProvider;

public static class DatabaseExtensions
{
    public static void Validate(this Microsoft.SqlServer.Management.Smo.Database database, DatabaseFileInfo fileInfo)
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

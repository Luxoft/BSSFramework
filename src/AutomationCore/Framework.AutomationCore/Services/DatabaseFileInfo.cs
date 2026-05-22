namespace Framework.AutomationCore.Services;

public record DatabaseFileInfo(string DbPath, string LogPath)
{
    public bool IsExists() => File.Exists(this.DbPath) && File.Exists(this.LogPath);
}

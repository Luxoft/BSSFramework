using Framework.Authorization.Domain;

namespace Framework.Authorization.Notification;

internal class PermissionLevelInfo
{
    public Permission Permission { get; set; }

    public string LevelInfo { get; set; }
}

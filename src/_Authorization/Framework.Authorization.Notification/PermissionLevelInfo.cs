using Framework.Authorization.Domain;

namespace Framework.Authorization.Notification;

internal class PermissionLevelInfo
{
    public required Permission Permission { get; init; }

    public required string LevelInfo { get; init; }
}

internal class FullPermissionLevelInfo : PermissionLevelInfo
{
    public required int Level { get; init; }
}

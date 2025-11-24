using Framework.Authorization.Domain;

namespace Framework.Authorization.Notification;

public class PermissionLevelInfo
{
    public required Permission Permission { get; init; }

    public required string LevelInfo { get; init; }
}

public class FullPermissionLevelInfo : PermissionLevelInfo
{
    public required int Level { get; init; }
}

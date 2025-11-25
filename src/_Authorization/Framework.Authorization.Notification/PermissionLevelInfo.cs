using Framework.Authorization.Domain;

namespace Framework.Authorization.Notification;

public record PermissionLevelInfo
{
    public required Permission Permission { get; init; }

    public required string LevelInfo { get; init; }
}

public record FullPermissionLevelInfo : PermissionLevelInfo
{
    public required int Level { get; init; }
}

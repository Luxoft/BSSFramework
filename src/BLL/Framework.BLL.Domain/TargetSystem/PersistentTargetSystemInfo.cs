namespace Framework.BLL.Domain.TargetSystem;

public record PersistentTargetSystemInfo : TargetSystemInfo
{
    public required bool IsMain { get; init; }

    public required bool IsRevision { get; init; }

    public required Type PersistentDomainObjectBaseType { get; init; }

    public required Type BllContextType { get; init; }
}

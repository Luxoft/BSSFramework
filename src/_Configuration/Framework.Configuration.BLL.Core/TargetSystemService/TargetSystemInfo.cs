namespace Framework.Configuration.BLL;

public abstract record TargetSystemInfo(Guid Id, bool IsMain, bool IsRevision, string Name, IReadOnlyList<DomainTypeInfo> DomainTypes);

public record TargetSystemInfo<TPersistentDomainObjectBase>(
    Guid Id,
    bool IsMain,
    bool IsRevision,
    string Name,
    IReadOnlyList<DomainTypeInfo> DomainTypes) : TargetSystemInfo(Id, IsMain, IsRevision, Name, DomainTypes);

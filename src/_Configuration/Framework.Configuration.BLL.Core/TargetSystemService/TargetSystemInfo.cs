namespace Framework.Configuration.BLL;

public abstract record TargetSystemInfo(string Name, Guid Id, bool IsMain, bool IsRevision, IReadOnlyList<DomainTypeInfo> DomainTypes);

public record TargetSystemInfo<TPersistentDomainObjectBase>(
    string Name,
    Guid Id,
    bool IsMain,
    bool IsRevision,
    IReadOnlyList<DomainTypeInfo> DomainTypes) : TargetSystemInfo(Name, Id, IsMain, IsRevision, DomainTypes);

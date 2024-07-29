namespace Framework.Configuration.BLL;

public abstract record TargetSystemInfo(bool IsMain, bool IsRevision, string Name);

public record TargetSystemInfo<TPersistentDomainObjectBase>(bool IsMain, bool IsRevision, string Name) : TargetSystemInfo(IsMain, IsRevision, Name);

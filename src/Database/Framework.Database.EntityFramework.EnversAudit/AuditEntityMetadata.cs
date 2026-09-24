namespace Framework.Database.EntityFramework.EnversAudit;

public sealed record AuditEntityMetadata(
    Type EntityType,
    Type AuditEntityType,
    IReadOnlyList<AuditPropertyMetadata> Properties);

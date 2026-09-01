namespace Framework.Database.EntityFramework.Audit;

public sealed record AuditEntityMetadata(
    Type EntityType,
    Type AuditEntityType,
    IReadOnlyList<AuditPropertyMetadata> Properties);

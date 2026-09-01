namespace Framework.Database.EntityFramework.Audit;

public interface IAuditEntityFactory
{
    string RevisionIdPropertyName { get; }

    string RevisionPropertyName { get; }

    string RevisionTypePropertyName { get; }

    AuditEntityMetadata GetOrCreate(Type entityType, IEnumerable<AuditPropertyMetadata> properties);

    bool TryGet(Type entityType, out AuditEntityMetadata metadata);
}

using NHibernate.Envers.Configuration.Attributes;
using NHibernate.Mapping;

namespace Framework.Database.NHibernate.Audit;

public interface IAuditAttributeService
{
    RelationTargetAuditMode GetAttributeFor(Type type);

    RelationTargetAuditMode GetAttributeFor(Type type, Property property);

    string GetAuditTableSchemaOrDefault(Type type);
}

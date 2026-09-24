using Framework.Database.InlineAudit;

namespace Framework.Database.NHibernate.InlineAudit;

public interface IAuditPropertiesSetterMapFactory
{
    IAuditPropertiesSetterMap CreateSetterMap(Type domainObjectType, InlineAuditAction inlineAuditAction, IReadOnlyList<string> propertyNames);
}

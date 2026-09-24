using Framework.Database.InlineAudit;

namespace Framework.Database.EntityFramework.InlineAudit;

public interface IAuditPropertiesSetterMapFactory
{
    IAuditPropertiesSetterMap CreateSetterMap(Type domainObjectType, InlineAuditAction inlineAuditAction);
}

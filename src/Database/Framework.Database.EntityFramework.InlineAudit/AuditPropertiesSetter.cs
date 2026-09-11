using Framework.Database.InlineAudit;

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Framework.Database.EntityFramework.InlineAudit;

public class AuditPropertiesSetter<TProperty>(IAuditValueResolver<TProperty> auditValueResolver, string propertyName) : IAuditPropertiesSetter
{
    public bool SetAuditFields(EntityEntry entry)
    {
        entry.Property(propertyName).CurrentValue = auditValueResolver.GetCurrentValue();

        return true;
    }
}

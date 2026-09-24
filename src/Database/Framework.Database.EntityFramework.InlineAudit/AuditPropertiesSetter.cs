using Framework.Database.InlineAudit;

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Framework.Database.EntityFramework.InlineAudit;

public class AuditPropertiesSetter<TProperty>(IAuditValueResolver<TProperty> auditValueResolver, string propertyName) : IAuditPropertiesSetter
{
    public bool SetAuditFields(EntityEntry entry)
    {
        var value = auditValueResolver.GetCurrentValue();

        if (entry.Metadata.FindNavigation(propertyName) != null)
        {
            entry.Reference(propertyName).CurrentValue = value;
        }
        else
        {
            entry.Property(propertyName).CurrentValue = value;
        }

        return true;
    }
}

using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Framework.Database.EntityFramework.InlineAudit;

public interface IAuditPropertiesSetter
{
    bool SetAuditFields(EntityEntry entry);
}

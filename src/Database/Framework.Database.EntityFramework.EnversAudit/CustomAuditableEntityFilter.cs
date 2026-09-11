using Microsoft.EntityFrameworkCore.Metadata;

namespace Framework.Database.EntityFramework.EnversAudit;

public class CustomAuditableEntityFilter(Func<IReadOnlyEntityType, bool> isAuditable) : IAuditableEntityFilter
{
    public bool IsAuditable(IReadOnlyEntityType entityType) => isAuditable(entityType);
}

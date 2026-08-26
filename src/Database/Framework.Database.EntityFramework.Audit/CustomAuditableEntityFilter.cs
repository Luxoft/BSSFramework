using Microsoft.EntityFrameworkCore.Metadata;

namespace Framework.Database.EntityFramework.Audit;

public class CustomAuditableEntityFilter(Func<IReadOnlyEntityType, bool> isAuditable) : IAuditableEntityFilter
{
    public bool IsAuditable(IReadOnlyEntityType entityType) => isAuditable(entityType);
}

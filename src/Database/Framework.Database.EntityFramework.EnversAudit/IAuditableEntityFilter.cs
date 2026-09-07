using Microsoft.EntityFrameworkCore.Metadata;

namespace Framework.Database.EntityFramework.EnversAudit;

public interface IAuditableEntityFilter
{
    bool IsAuditable(IReadOnlyEntityType entityType);
}

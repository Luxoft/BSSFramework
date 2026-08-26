using Microsoft.EntityFrameworkCore.Metadata;

namespace Framework.Database.EntityFramework.Audit;

public interface IAuditableEntityFilter
{
    bool IsAuditable(IReadOnlyEntityType entityType);
}

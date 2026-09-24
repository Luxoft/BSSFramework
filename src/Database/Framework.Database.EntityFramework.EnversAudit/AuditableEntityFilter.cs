using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Framework.Database.EntityFramework.EnversAudit;

public class AuditableEntityFilter : IAuditableEntityFilter
{
    public bool IsAuditable(IReadOnlyEntityType entityType)
    {
        if (entityType.GetTableName() is { } tableName)
        {
            if (tableName.EndsWith("Audit"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return true;
        }
    }
}

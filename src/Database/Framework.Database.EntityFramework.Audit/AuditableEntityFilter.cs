using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Framework.Database.EntityFramework.Audit;

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

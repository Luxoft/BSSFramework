using Microsoft.EntityFrameworkCore.Metadata;

namespace Framework.Database.EntityFramework.EnversAudit;

public interface IAuditInfoResolver
{
    (string SchemaName, string TableName) GetInfo(IReadOnlyTypeBase entityType);
}

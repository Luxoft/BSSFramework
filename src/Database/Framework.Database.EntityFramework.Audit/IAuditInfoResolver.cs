using Microsoft.EntityFrameworkCore.Metadata;

namespace Framework.Database.EntityFramework.Audit;

public interface IAuditInfoResolver
{
    (string SchemaName, string TableName) GetInfo(IReadOnlyTypeBase entityType);
}

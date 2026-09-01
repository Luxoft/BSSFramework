using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Framework.Database.EntityFramework.Audit;

public class AuditInfoResolver(IAuditTypeNameResolver auditTypeNameResolver) : IAuditInfoResolver
{
    public (string SchemaName, string TableName) GetInfo(IReadOnlyTypeBase entityType) =>
        ($"{entityType.GetSchema()}Audit", auditTypeNameResolver.GetName(entityType.ClrType));
}

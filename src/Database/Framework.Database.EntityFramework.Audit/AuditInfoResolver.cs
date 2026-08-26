using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Framework.Database.EntityFramework.Audit;

public class AuditInfoResolver : IAuditInfoResolver
{
    public (string SchemaName, string TableName) GetInfo(IReadOnlyTypeBase entityType) =>
        ($"{entityType.GetSchema()}Audit", $"{entityType.ClrType.Name}Audit");
}

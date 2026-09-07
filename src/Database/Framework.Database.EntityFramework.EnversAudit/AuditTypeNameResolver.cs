namespace Framework.Database.EntityFramework.EnversAudit;

public class AuditTypeNameResolver : IAuditTypeNameResolver
{
    public string GetName(Type type) => $"{type.Name}Audit";
}

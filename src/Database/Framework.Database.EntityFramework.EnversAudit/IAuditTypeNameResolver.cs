namespace Framework.Database.EntityFramework.EnversAudit;

public interface IAuditTypeNameResolver
{
    string GetName(Type type);
}

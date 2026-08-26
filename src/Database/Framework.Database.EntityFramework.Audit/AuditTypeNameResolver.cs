namespace Framework.Database.EntityFramework.Audit;

public class AuditTypeNameResolver : IAuditTypeNameResolver
{
    public string GetName(Type type) => $"{type.Name}Audit";
}

namespace Framework.Database.EntityFramework.Audit;

public interface IAuditTypeNameResolver
{
    string GetName(Type type);
}

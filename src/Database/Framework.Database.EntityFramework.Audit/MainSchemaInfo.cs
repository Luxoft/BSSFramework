namespace Framework.Database.EntityFramework.Audit;

public record MainAuditSchemaInfo(string Name)
{
    public static MainAuditSchemaInfo Default = new("AppAudit");
}

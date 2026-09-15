namespace Framework.Database.EntityFramework.EnversAudit;

public record MainAuditSchemaInfo(string Name)
{
    public static MainAuditSchemaInfo Default = new("AppAudit");
}

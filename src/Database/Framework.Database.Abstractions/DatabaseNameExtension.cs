namespace Framework.Database;

public static class DatabaseNameExtension
{
    /// <summary>
    /// Creating the default instance of <see cref="AuditDatabaseName"/>
    /// Which uses the appAudit schema to store audit
    /// </summary>
    public static AuditDatabaseName ToDefaultAudit(this DatabaseName source) => new(source.Name, source.Schema + "Audit", "appAudit");
}

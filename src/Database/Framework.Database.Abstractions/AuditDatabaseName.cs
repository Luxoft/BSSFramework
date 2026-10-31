namespace Framework.Database;

/// <summary>
/// Description of audit storage settings in the database
/// </summary>
public record AuditDatabaseName(string Name, string Schema, string RevisionEntitySchema) : DatabaseName(Name, Schema);

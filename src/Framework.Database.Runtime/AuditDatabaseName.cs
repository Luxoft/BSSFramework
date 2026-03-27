namespace Framework.Database;

/// <summary>
/// Описание настроек хранение аудита в базе данных
/// </summary>
public record AuditDatabaseName(string Name, string Schema, string RevisionEntitySchema) : DatabaseName(Name, Schema);

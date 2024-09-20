namespace Framework.DomainDriven;

/// <summary>
/// Описание настроек хранение аудита в базе данных
/// </summary>
public record AuditDatabaseName(string Name, string Schema, string RevisionEntitySchema) : DatabaseName(Name, Schema);

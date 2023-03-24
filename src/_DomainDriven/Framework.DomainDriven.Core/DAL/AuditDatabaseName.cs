namespace Framework.DomainDriven;

/// <summary>
/// Описание настроек хранение аудита в базе данных
/// </summary>
public class AuditDatabaseName : DatabaseName
{
    public AuditDatabaseName(string name, string schema, string revisionEntitySchema)
            : base(name, schema)
    {
        this.RevisionEntitySchema = revisionEntitySchema;
    }

    /// <summary>
    /// Схема в которой будет расположена таблица AuditRevisionEntity
    /// </summary>
    public string RevisionEntitySchema { get; }
}

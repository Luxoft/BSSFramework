#nullable enable

namespace Framework.DomainDriven;

/// <summary>
/// Описание настроек хранение аудита в базе данных
/// </summary>
public record AuditDatabaseName : DatabaseName
{
    public string RevisionEntitySchema { get; init; } = "appAudit";
}

#nullable enable

namespace Framework.DomainDriven.NHibernate;

public record MappingSettings(
    Type PersistentDomainObjectBaseType,
    IConfigurationInitializer Initializer,
    DatabaseName Database,
    AuditDatabaseName? AuditDatabase,
    IReadOnlyList<Type> Types)
{
    public IAuditTypeFilter AuditTypeFilter { get; init; } = new DefaultAuditTypeFilter();
}

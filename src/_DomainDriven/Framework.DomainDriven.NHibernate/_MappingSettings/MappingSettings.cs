#nullable enable

using System.Reflection;
using System.Xml.Linq;

using Framework.Persistent;

namespace Framework.DomainDriven.NHibernate;

public record MappingSettings<TPersistentDomainObjectBase>(
    IConfigurationInitializer Initializer,
    DatabaseName Database,
    AuditDatabaseName? AuditDatabase,
    IReadOnlyList<Type> Types) : MappingSettings(
    typeof(TPersistentDomainObjectBase),
    Initializer,
    Database,
    AuditDatabase,
    Types)
{
    public MappingSettings(
        IConfigurationInitializer initializer,
        DatabaseName database,
        AuditDatabaseName? auditDatabase,
        IEnumerable<Type>? types = null)
        : this(initializer, database, auditDatabase, types?.ToList() ?? GetDefaultTypes())
    {
    }

    public MappingSettings(Assembly mappingAssembly, IEnumerable<Type>? types = null)
        : this(mappingAssembly, new DatabaseName(typeof(TPersistentDomainObjectBase).ExtractSystemName()), types)
    {
    }

    public MappingSettings(Assembly mappingAssembly, DatabaseName databaseName, IEnumerable<Type>? types = null)
        : this(mappingAssembly, databaseName, null, types)
    {
    }

    public MappingSettings(
        Assembly mappingAssembly,
        DatabaseName databaseName,
        AuditDatabaseName? auditDatabaseName,
        IEnumerable<Type>? types = null)
        : this(
            new MappingSettingsInitializer(databaseName, mappingAssembly),
            databaseName,
            auditDatabaseName,
            types)
    {
    }

    public MappingSettings(
        IEnumerable<XDocument> mappingXmls,
        DatabaseName databaseName,
        bool auditDatabaseName,
        IEnumerable<Type>? types = null)
        : this(mappingXmls, databaseName, auditDatabaseName ? databaseName.ToDefaultAudit() : null, types)
    {
    }

    public MappingSettings(
        IEnumerable<XDocument> mappingXmls,
        DatabaseName databaseName,
        AuditDatabaseName? auditDatabaseName,
        IEnumerable<Type>? types = null)
        : this(
            new MappingSettingsInitializer(databaseName, mappingXmls),
            databaseName,
            auditDatabaseName,
            types)
    {
    }

    public MappingSettings(IEnumerable<XDocument> mappingXmls, bool isAudit)
        : this(
            mappingXmls,
            new DatabaseName(typeof(TPersistentDomainObjectBase).ExtractSystemName()),
            isAudit ? new DatabaseName(typeof(TPersistentDomainObjectBase).ExtractSystemName()).ToDefaultAudit() : null)
    {
    }

    public MappingSettings(IConfigurationInitializer initializer, DatabaseName databaseName, bool isAudit, IEnumerable<Type>? types = null)
        : this(initializer, databaseName, isAudit ? databaseName.ToDefaultAudit() : null, types)
    {
    }

    private static IReadOnlyList<Type> GetDefaultTypes()
    {
        return typeof(TPersistentDomainObjectBase).Assembly
                                                  .GetTypes()
                                                  .Where(typeof(TPersistentDomainObjectBase).IsAssignableFrom)
                                                  .ToList();
    }
}

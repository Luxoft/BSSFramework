#nullable enable

using System.Reflection;
using System.Xml.Linq;

namespace Framework.DomainDriven.NHibernate;

public record MappingSettings<TPersistentDomainObjectBase>(
    IConfigurationInitializer Initializer,
    DatabaseName Database,
    AuditDatabaseName? AuditDatabase) : MappingSettings(
    typeof(TPersistentDomainObjectBase),
    Initializer,
    Database,
    AuditDatabase,
    GetDefaultTypes())
{
    public MappingSettings(DatabaseName databaseName, AuditDatabaseName? auditDatabaseName = null)
        : this(ConfigurationInitializer.Empty, databaseName, auditDatabaseName)
    {
    }

    public MappingSettings(Assembly mappingAssembly, DatabaseName databaseName, AuditDatabaseName? auditDatabaseName = null)
        : this(new MappingSettingsInitializer(databaseName, mappingAssembly), databaseName, auditDatabaseName)
    {
    }

    public MappingSettings(
        IEnumerable<XDocument> mappingXmls,
        DatabaseName databaseName,
        AuditDatabaseName? auditDatabaseName = null)
        : this(
            new MappingSettingsInitializer(databaseName, mappingXmls),
            databaseName,
            auditDatabaseName)
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

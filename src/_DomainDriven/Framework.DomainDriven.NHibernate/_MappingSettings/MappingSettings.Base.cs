#nullable enable

using System.Collections.ObjectModel;
using System.Reflection;
using System.Xml.Linq;

using Framework.Core;

namespace Framework.DomainDriven.NHibernate;

public class MappingSettings : IMappingSettings
{
    public MappingSettings(
        Type persistentDomainObjectBaseType,
        Assembly mappingAssembly,
        DatabaseName databaseName,
        AuditDatabaseName? auditDatabaseName = null,
        IEnumerable<Type>? types = null)
        : this(
            persistentDomainObjectBaseType,
            new MappingSettingsInitializer(databaseName, mappingAssembly),
            databaseName,
            auditDatabaseName,
            types)
    {
    }

    public MappingSettings(
        Type persistentDomainObjectBaseType,
        IEnumerable<XDocument> mappingXmls,
        DatabaseName databaseName,
        AuditDatabaseName? auditDatabaseName = null,
        IEnumerable<Type>? types = null)
        : this(
            persistentDomainObjectBaseType,
            new MappingSettingsInitializer(databaseName, mappingXmls),
            databaseName,
            auditDatabaseName,
            types)
    {
    }

    public MappingSettings(
        Type persistentDomainObjectBaseType,
        IConfigurationInitializer initializer,
        DatabaseName databaseName,
        AuditDatabaseName? auditDatabaseName = null,
        IEnumerable<Type>? types = null)
    {
        this.PersistentDomainObjectBaseType = persistentDomainObjectBaseType;
        this.Initializer = initializer;
        this.Database = databaseName;
        this.AuditDatabase = auditDatabaseName;

        this.Types = (types
                      ?? this.PersistentDomainObjectBaseType.Assembly.GetTypes()
                             .Where(this.PersistentDomainObjectBaseType.IsAssignableFrom)).ToReadOnlyCollection();
    }


    public Type PersistentDomainObjectBaseType { get; }

    public DatabaseName Database { get; }

    public AuditDatabaseName? AuditDatabase { get; }

    public ReadOnlyCollection<Type> Types { get; }

    public IConfigurationInitializer Initializer { get; }

    public IAuditTypeFilter GetAuditTypeFilter() => new DefaultAuditTypeFilter();
}

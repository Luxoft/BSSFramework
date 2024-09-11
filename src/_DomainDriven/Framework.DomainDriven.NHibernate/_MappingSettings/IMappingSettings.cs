#nullable enable

using System.Collections.ObjectModel;

namespace Framework.DomainDriven.NHibernate;

public interface IMappingSettings
{
    DatabaseName Database { get; }

    AuditDatabaseName? AuditDatabase { get; }

    Type PersistentDomainObjectBaseType { get; }

    ReadOnlyCollection<Type> Types { get; }

    IConfigurationInitializer Initializer { get; }

    IAuditTypeFilter GetAuditTypeFilter();
}

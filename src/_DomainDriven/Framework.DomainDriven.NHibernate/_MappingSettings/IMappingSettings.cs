using System.Collections.ObjectModel;

using NHibernate.Cfg;

namespace Framework.DomainDriven.NHibernate;

public interface IMappingSettings : IPersistentDomainObjectBaseTypeContainer
{
    DatabaseName Database { get; }

    AuditDatabaseName AuditDatabase { get; }

    ReadOnlyCollection<Type> Types { get; }

    void InitMapping(Configuration cfg);

    IAuditTypeFilter GetAuditTypeFilter();
}

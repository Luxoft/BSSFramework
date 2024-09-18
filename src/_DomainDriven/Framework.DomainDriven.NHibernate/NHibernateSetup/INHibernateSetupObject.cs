using System.Data;
using System.Reflection;
using FluentNHibernate.Cfg;

namespace Framework.DomainDriven.NHibernate;

public interface INHibernateSetupObject
{
    bool AddDefaultListener { get; set; }

    bool AddDefaultInitializer { get; set; }

    bool AutoAddFluentMapping { get; set; }

    INHibernateSetupObject AddMapping<TMappingSettings>()
        where TMappingSettings : MappingSettings;

    INHibernateSetupObject AddMapping(MappingSettings mapping);

    INHibernateSetupObject AddInitializer<TInitializer>()
        where TInitializer : class, IConfigurationInitializer;

    INHibernateSetupObject AddEventListener<TEventListener>()
        where TEventListener : class, IDBSessionEventListener;

    INHibernateSetupObject AddFluentMapping(Assembly assembly);

    INHibernateSetupObject WithInitFluent(Action<FluentMappingsContainer> initAction);

    INHibernateSetupObject SetIsolationLevel(IsolationLevel isolationLevel);

    INHibernateSetupObject SetSqlTypesKeepDateTime(bool value);

    INHibernateSetupObject SetCommandTimeout(int timeout);

    INHibernateSetupObject SetDefaultConnectionStringName(string connectionStringName);
}

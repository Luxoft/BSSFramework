using System.Reflection;

namespace Framework.DomainDriven.NHibernate;

public interface INHibernateSetupObject
{
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

    INHibernateSetupObject SetDefaultConnectionStringName(string connectionStringName);
}

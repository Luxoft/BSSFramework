namespace Framework.DomainDriven.NHibernate;

public interface INHibernateSetupObject
{
    INHibernateSetupObject SetEnvironment<TNHibSessionEnvironment>()
        where TNHibSessionEnvironment : NHibSessionEnvironment;

    INHibernateSetupObject SetEnvironment(NHibSessionEnvironment sessionEnvironment);

    INHibernateSetupObject AddMapping<TMappingSettings>()
        where TMappingSettings : MappingSettings;

    INHibernateSetupObject AddMapping(MappingSettings mapping);

    INHibernateSetupObject AddInitializer(IConfigurationInitializer configurationInitializer);

    INHibernateSetupObject AddEventListener<TEventListener>()
        where TEventListener : class, IDBSessionEventListener;
}

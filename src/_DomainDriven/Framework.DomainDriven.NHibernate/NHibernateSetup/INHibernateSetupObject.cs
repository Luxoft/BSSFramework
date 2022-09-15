namespace Framework.DomainDriven.NHibernate;

public interface INHibernateSetupObject
{
    INHibernateSetupObject SetEnvironment<TNHibSessionEnvironment>()
            where TNHibSessionEnvironment : NHibSessionEnvironment;

    INHibernateSetupObject SetEnvironment(NHibSessionEnvironment sessionEnvironment);

    INHibernateSetupObject AddMapping<TMappingSettings>()
            where TMappingSettings : class, IMappingSettings;

    INHibernateSetupObject AddMapping(IMappingSettings mapping);

    INHibernateSetupObject AddEventListener<TEventListener>()
            where TEventListener : class, IDBSessionEventListener;
}

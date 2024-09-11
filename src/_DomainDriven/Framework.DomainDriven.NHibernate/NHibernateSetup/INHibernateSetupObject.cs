namespace Framework.DomainDriven.NHibernate;

public interface INHibernateSetupObject
{
    INHibernateSetupObject SetEnvironment<TNHibSessionEnvironment>()
        where TNHibSessionEnvironment : NHibSessionEnvironment;

    INHibernateSetupObject SetEnvironment(NHibSessionEnvironment sessionEnvironment);

    INHibernateSetupObject AddMapping(IMappingSettings mapping);

    INHibernateSetupObject AddInitializer(IConfigurationInitializer configurationInitializer);

    INHibernateSetupObject AddEventListener<TEventListener>()
        where TEventListener : class, IDBSessionEventListener;
}

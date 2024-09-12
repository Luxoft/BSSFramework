using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.NHibernate;

public class NHibernateSetupObject : INHibernateSetupObject
{
    private List<Action<IServiceCollection>> initActions = new();

    public Action<IServiceCollection> SetEnvironmentAction { get; private set; } = sc => sc.AddSingleton<NHibSessionEnvironment>();

    public IReadOnlyList<Action<IServiceCollection>> InitActions => this.initActions;

    public INHibernateSetupObject SetEnvironment<TNHibSessionEnvironment>()
        where TNHibSessionEnvironment : NHibSessionEnvironment
    {
        this.SetEnvironmentAction = sc => sc.AddSingleton<NHibSessionEnvironment, TNHibSessionEnvironment>();

        return this;
    }

    public INHibernateSetupObject SetEnvironment(NHibSessionEnvironment sessionEnvironment)
    {
        this.SetEnvironmentAction = sc => sc.AddSingleton(sessionEnvironment);

        return this;
    }

    public INHibernateSetupObject AddMapping<TMappingSettings>()
            where TMappingSettings : MappingSettings
    {
        this.initActions.Add(sc => sc.AddSingleton<MappingSettings, TMappingSettings>());

        return this;
    }

    public INHibernateSetupObject AddMapping(MappingSettings mapping)
    {
        this.initActions.Add(sc => sc.AddSingleton(mapping));

        return this;
    }

    public INHibernateSetupObject AddEventListener<TEventListener>()
            where TEventListener : class, IDBSessionEventListener
    {
        this.initActions.Add(sc => sc.AddScoped<IDBSessionEventListener, TEventListener>());

        return this;
    }
}

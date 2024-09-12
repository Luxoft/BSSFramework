using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.NHibernate;

public class NHibernateSetupObject : INHibernateSetupObject
{
    private readonly List<Action<IServiceCollection>> initActions = new();

    public string DefaultConnectionStringName { get; private set; } = "DefaultConnection";

    public bool AddDefaultInitializer { get; set; } = true;

    public bool AutoAddFluentMapping { get; set; } = true;

    public INHibernateSetupObject AddMapping<TMappingSettings>()
            where TMappingSettings : MappingSettings
    {
        this.initActions.Add(services =>
                             {
                                 services.AddSingleton<MappingSettings, TMappingSettings>();
                                 this.TryAddFluentMapping(services, typeof(TMappingSettings).Assembly);
                             });

        return this;
    }

    public INHibernateSetupObject AddMapping(MappingSettings mapping)
    {
        this.initActions.Add(services =>
                             {
                                 services.AddSingleton(mapping);
                                 this.TryAddFluentMapping(services, mapping.GetType().Assembly);
                             });

        return this;
    }

    public INHibernateSetupObject AddInitializer<TInitializer>()
        where TInitializer : class, IConfigurationInitializer
    {
        this.initActions.Add(services => services.AddSingleton<IConfigurationInitializer, TInitializer>());

        return this;
    }

    public INHibernateSetupObject AddEventListener<TEventListener>()
        where TEventListener : class, IDBSessionEventListener
    {
        this.initActions.Add(services => services.AddScoped<IDBSessionEventListener, TEventListener>());

        return this;
    }

    public INHibernateSetupObject AddFluentMapping(Assembly assembly)
    {
        this.initActions.Add(services => services.AddSingleton(new FluentMappingAssemblyInfo(assembly)));

        return this;
    }

    public INHibernateSetupObject SetDefaultConnectionStringName(string connectionStringName)
    {
        this.DefaultConnectionStringName = connectionStringName;

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        services.AddSingleton(new DefaultConnectionStringSettings(this.DefaultConnectionStringName));

        if (this.AddDefaultInitializer)
        {
            this.AddInitializer<DefaultConfigurationInitializer>();
        }

        foreach (var action in this.initActions)
        {
            action(services);
        }
    }

    private void TryAddFluentMapping(IServiceCollection services, Assembly assembly)
    {
        if (this.AutoAddFluentMapping)
        {
            services.AddSingleton(new FluentMappingAssemblyInfo(assembly));
        }
    }
}

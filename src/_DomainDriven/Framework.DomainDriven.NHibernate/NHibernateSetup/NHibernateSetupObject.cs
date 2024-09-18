using System.Data;
using System.Reflection;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.NHibernate;

public class NHibernateSetupObject : INHibernateSetupObject
{
    private readonly List<Assembly> autoMappingAssemblies = new();

    private DefaultConfigurationInitializerSettings settings = new();

    private readonly List<Action<IServiceCollection>> initActions = new();

    private string defaultConnectionStringName = "DefaultConnection";

    public bool AddDefaultInitializer { get; set; } = true;

    public bool AddDefaultListener { get; set; } = true;

    public bool AutoAddFluentMapping { get; set; } = true;

    public INHibernateSetupObject AddMapping<TMappingSettings>()
        where TMappingSettings : MappingSettings
    {
        this.initActions.Add(services => services.AddSingleton<MappingSettings, TMappingSettings>());

        this.autoMappingAssemblies.Add(typeof(TMappingSettings).Assembly);

        return this;
    }

    public INHibernateSetupObject AddMapping(MappingSettings mapping)
    {
        this.initActions.Add(services => services.AddSingleton(mapping));

        this.autoMappingAssemblies.Add(mapping.GetType().Assembly);

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
        this.settings = this.settings with { FluentAssemblyList = this.settings.FluentAssemblyList.Union([assembly]).ToList() };

        return this;
    }

    public INHibernateSetupObject WithRawMapping(Action<MappingConfiguration> initAction)
    {
        var prevAction = this.settings.RawMappingAction;

        this.settings = this.settings with
                        {
                            RawMappingAction = v =>
                                               {
                                                   prevAction(v);
                                                   initAction(v);
                                               }
                        };

        return this;
    }

    public INHibernateSetupObject WithRawDatabase(Action<MsSqlConfiguration> initAction)
    {
        var prevAction = this.settings.RawDatabaseAction;

        this.settings = this.settings with
                        {
                            RawDatabaseAction = v =>
                                                {
                                                    prevAction(v);
                                                    initAction(v);
                                                }
                        };

        return this;
    }

    public INHibernateSetupObject SetIsolationLevel(IsolationLevel isolationLevel)
    {
        this.settings = this.settings with { IsolationLevel = isolationLevel };

        return this;
    }

    public INHibernateSetupObject SetBatchSize(int batchSize)
    {
        this.settings = this.settings with { BatchSize = batchSize };

        return this;
    }

    public INHibernateSetupObject SetComponentConvention(bool enabled)
    {
        this.settings = this.settings with { ComponentConventionEnable = enabled };

        return this;
    }

    public INHibernateSetupObject SetSqlTypesKeepDateTime(bool value)
    {
        this.settings = this.settings with { SqlTypesKeepDateTime = value };

        return this;
    }

    public INHibernateSetupObject SetCommandTimeout(int timeout)
    {
        this.settings = this.settings with { CommandTimeout = timeout };

        return this;
    }

    public INHibernateSetupObject SetDefaultConnectionStringName(string connectionStringName)
    {
        this.defaultConnectionStringName = connectionStringName;

        return this;
    }


    public void Initialize(IServiceCollection services)
    {
        services.AddSingleton(new DefaultConnectionStringSettings(this.defaultConnectionStringName));

        if (this.AddDefaultListener)
        {
            this.AddEventListener<DefaultDBSessionEventListener>();
        }

        if (this.AddDefaultInitializer)
        {
            if (this.AutoAddFluentMapping)
            {
                foreach (var assembly in this.autoMappingAssemblies)
                {
                    this.AddFluentMapping(assembly);
                }
            }

            services.AddSingleton(this.settings);
            this.AddInitializer<DefaultConfigurationInitializer>();
        }

        foreach (var action in this.initActions)
        {
            action(services);
        }
    }
}

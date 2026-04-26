using System.Reflection;

using Anch.DependencyInjection;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

using Framework.Core;
using Framework.Database.NHibernate.Mapping;
using Framework.Database.NHibernate.Sessions;
using Framework.Database.NHibernate.Visitors;
using Framework.DependencyInjection;

using Anch.GenericQueryable.NHibernate;

using Microsoft.Extensions.DependencyInjection;

using NHibernate;

namespace Framework.Database.NHibernate.DependencyInjection;

public class NHibernateSetup : INHibernateSetup, IServiceInitializer
{
    private readonly List<INHibernateSetupExtension> extensions = [];

    private readonly List<Assembly> autoMappingAssemblies = [];

    private NHibernateSettings settings = new();

    private readonly List<Action<IServiceCollection>> initActions = [];

    public bool AddDefaultInitializer { get; set; } = true;

    public bool AutoAddFluentMapping { get; set; } = true;

    public INHibernateSetup AddMapping<TMappingSettings>()
        where TMappingSettings : MappingSettings
    {
        this.initActions.Add(services => services.AddSingleton<MappingSettings, TMappingSettings>());

        this.autoMappingAssemblies.Add(typeof(TMappingSettings).Assembly);

        return this;
    }

    public INHibernateSetup AddMapping(MappingSettings mapping)
    {
        this.initActions.Add(services => services.AddSingleton(mapping));

        this.autoMappingAssemblies.Add(mapping.GetType().Assembly);

        return this;
    }

    public INHibernateSetup AddInitializer<TInitializer>()
        where TInitializer : class, IConfigurationInitializer
    {
        this.initActions.Add(services => services.AddSingleton<IConfigurationInitializer, TInitializer>());

        return this;
    }

    public INHibernateSetup AddFluentMapping(Assembly assembly)
    {
        this.settings = this.settings with { FluentAssemblyList = this.settings.FluentAssemblyList.Union([assembly]).ToList() };

        return this;
    }

    public INHibernateSetup WithRawMapping(Action<MappingConfiguration> initAction)
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

    public INHibernateSetup WithRawDatabase(Action<MsSqlConfiguration> initAction)
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

    public INHibernateSetup SetComponentConvention(bool enabled)
    {
        this.settings = this.settings with { ComponentConventionEnable = enabled };

        return this;
    }

    public INHibernateSetup SetSqlTypesKeepDateTime(bool value)
    {
        this.settings = this.settings with { SqlTypesKeepDateTime = value };

        return this;
    }

    public INHibernateSetup AddExtension(INHibernateSetupExtension extension)
    {
        this.extensions.Add(extension);

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        services.AddScoped(typeof(IAsyncDal<,>), typeof(NHibAsyncDal<,>));

        services.AddNHibernateGenericQueryable();

        //For close db session by middleware
        services.AddScopedFromLazyObject<INHibSession, NHibSession>();
        services.AddScopedFrom<ILazyObject<IDBSession>, ILazyObject<INHibSession>>();

        services.AddScopedFrom<ISession, INHibSession>(session => session.NativeSession);

        services.AddSingleton<INHibSessionEnvironmentSettings, NHibSessionEnvironmentSettings>();

        services.AddSingleton<NHibSessionEnvironment>();

        services.AddKeyedSingleton<IExpressionVisitorContainer>(IExpressionVisitorContainer.ElementKey, new ExpressionVisitorContainer(new FixNHibArrayContainsVisitor()));
        services.AddKeyedSingleton<IExpressionVisitorContainer, MathExpressionVisitorContainer>(IExpressionVisitorContainer.ElementKey);

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

        this.extensions.ForEach(ex => ex.AddServices(services));
    }
}

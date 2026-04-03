using System.Reflection;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

using Framework.Database.NHibernate._MappingSettings;

namespace Framework.Database.NHibernate.DependencyInjection;

public interface INHibernateSetup
{
    bool AddDefaultInitializer { get; set; }

    bool AutoAddFluentMapping { get; set; }

    INHibernateSetup AddMapping<TMappingSettings>()
        where TMappingSettings : MappingSettings;

    INHibernateSetup AddMapping(MappingSettings mapping);

    INHibernateSetup AddInitializer<TInitializer>()
        where TInitializer : class, IConfigurationInitializer;

    INHibernateSetup AddFluentMapping(Assembly assembly);

    INHibernateSetup WithRawMapping(Action<MappingConfiguration> initAction);

    INHibernateSetup WithRawDatabase(Action<MsSqlConfiguration> initAction);

    INHibernateSetup SetComponentConvention(bool enabled);

    INHibernateSetup SetSqlTypesKeepDateTime(bool value);

    INHibernateSetup AddExtension(INHibernateSetupExtension extension);
}

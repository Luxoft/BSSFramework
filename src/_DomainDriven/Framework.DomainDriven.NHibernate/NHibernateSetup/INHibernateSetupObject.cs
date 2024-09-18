using System.Data;
using System.Reflection;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

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

    INHibernateSetupObject WithRawMapping(Action<MappingConfiguration> initAction);

    INHibernateSetupObject WithRawDatabase(Action<MsSqlConfiguration> initAction);

    INHibernateSetupObject SetIsolationLevel(IsolationLevel isolationLevel);

    INHibernateSetupObject SetBatchSize(int batchSize);

    INHibernateSetupObject SetComponentConvention(bool enabled);

    INHibernateSetupObject SetSqlTypesKeepDateTime(bool value);

    INHibernateSetupObject SetCommandTimeout(int timeout);

    INHibernateSetupObject SetDefaultConnectionStringName(string connectionStringName);
}

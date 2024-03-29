﻿using System.Collections.ObjectModel;
using System.Reflection;
using System.Xml.Linq;

using Framework.Core;

using NHibernate.Cfg;

namespace Framework.DomainDriven.NHibernate;

public class MappingSettings : IMappingSettings
{
    private readonly Action<Configuration> _initMappingAction;

    public MappingSettings(
            Type persistentDomainObjectBaseType,
            Assembly mappingAssembly,
            DatabaseName databaseName,
            AuditDatabaseName auditDatabaseName = null,
            IEnumerable<Type> types = null)
            : this(persistentDomainObjectBaseType, GetInitAction(mappingAssembly, databaseName), databaseName, auditDatabaseName, types)
    {
    }

    public MappingSettings(
            Type persistentDomainObjectBaseType,
            IEnumerable<XDocument> mappingXmls,
            DatabaseName databaseName,
            AuditDatabaseName auditDatabaseName = null,
            IEnumerable<Type> types = null)
            : this(persistentDomainObjectBaseType, GetInitAction(mappingXmls, databaseName), databaseName, auditDatabaseName, types)
    {
    }

    public MappingSettings(
            Type persistentDomainObjectBaseType,
            Action<Configuration> initMappingAction,
            DatabaseName databaseName,
            AuditDatabaseName auditDatabaseName = null,
            IEnumerable<Type> types = null)
    {
        if (persistentDomainObjectBaseType == null) throw new ArgumentNullException(nameof(persistentDomainObjectBaseType));
        if (initMappingAction == null) throw new ArgumentNullException(nameof(initMappingAction));
        if (databaseName == null) throw new ArgumentNullException(nameof(databaseName));

        this.PersistentDomainObjectBaseType = persistentDomainObjectBaseType;

        this._initMappingAction = initMappingAction;

        this.Database = databaseName;
        this.AuditDatabase = auditDatabaseName;
        this.Types = (types ?? this.PersistentDomainObjectBaseType.Assembly.GetTypes().Where(this.PersistentDomainObjectBaseType.IsAssignableFrom)).ToReadOnlyCollection();
    }


    public Type PersistentDomainObjectBaseType { get; }

    public DatabaseName Database { get; }

    public AuditDatabaseName AuditDatabase { get; }

    public ReadOnlyCollection<Type> Types { get; }


    public virtual void InitMapping(Configuration cfg)
    {
        if (cfg == null) throw new ArgumentNullException(nameof(cfg));

        this._initMappingAction(cfg);
    }

    public IAuditTypeFilter GetAuditTypeFilter()
    {
        return new DefaultAuditTypeFilter();
    }

    private static Action<Configuration> GetInitAction(Assembly mappingAssembly, DatabaseName database)
    {
        if (mappingAssembly == null) throw new ArgumentNullException(nameof(mappingAssembly));

        var targetSchema = database.ToString();

        return cfg =>
               {
                   Action<object, BindMappingEventArgs> action = (sender, e) => { e.Mapping.schema = targetSchema; };

                   var eventHandler = new EventHandler<BindMappingEventArgs>(action);

                   cfg.BeforeBindMapping += eventHandler;

                   cfg.AddAssembly(mappingAssembly);

                   cfg.BeforeBindMapping -= eventHandler;
               };
    }

    private static Action<Configuration> GetInitAction(IEnumerable<XDocument> mappingXmls, DatabaseName database)
    {
        if (mappingXmls == null) throw new ArgumentNullException(nameof(mappingXmls));


        var cachedMappingXmls = mappingXmls.ToArray();

        var targetSchema = database.ToString();

        return cfg =>
               {
                   Action<object, BindMappingEventArgs> action = (sender, e) => { e.Mapping.schema = targetSchema; };

                   var eventHandler = new EventHandler<BindMappingEventArgs>(action);

                   cfg.BeforeBindMapping += eventHandler;

                   cfg.AddDocuments(cachedMappingXmls);

                   cfg.BeforeBindMapping -= eventHandler;
               };
    }
}

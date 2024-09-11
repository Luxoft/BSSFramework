﻿#nullable enable

using System.Reflection;
using System.Xml.Linq;

using NHibernate.Cfg;

namespace Framework.DomainDriven.NHibernate;

public class MappingSettingsInitializer : IConfigurationInitializer
{
    private readonly Action<Configuration> initAction;

    public MappingSettingsInitializer(DatabaseName database, Assembly mappingAssembly) => this.initAction = GetInitAction(database, cfg => cfg.AddAssembly(mappingAssembly));

    public MappingSettingsInitializer(DatabaseName database, IEnumerable<XDocument> mappingXmls)
    {
        var cachedMappingXmls = mappingXmls.ToArray();

        this.initAction = GetInitAction(database, cfg => cfg.AddDocuments(cachedMappingXmls));
    }

    public void Initialize(Configuration cfg) => this.initAction(cfg);


    private static Action<Configuration> GetInitAction(DatabaseName database, Action<Configuration> initMapping)
    {
        var targetSchema = database.ToString();

        return cfg =>
               {
                   var eventHandler = new EventHandler<BindMappingEventArgs>((_, e) => { e.Mapping.schema = targetSchema; });

                   cfg.BeforeBindMapping += eventHandler;

                   initMapping(cfg);

                   cfg.BeforeBindMapping -= eventHandler;
               };
    }
}

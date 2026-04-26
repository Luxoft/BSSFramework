using System.Reflection;
using System.Xml.Linq;

using Anch.Core;

using NHibernate.Cfg;

namespace Framework.Database.NHibernate.Mapping;

public class MappingSettingsInitializer : ConfigurationInitializer
{
    public MappingSettingsInitializer(DatabaseName database, Assembly mappingAssembly)
        : base(GetInitAction(database, cfg => cfg.AddAssembly(mappingAssembly)))
    {
    }

    public MappingSettingsInitializer(DatabaseName database, IEnumerable<XDocument> mappingXmls)
        : base(mappingXmls.ToArray().Pipe(cachedMappingXmls => GetInitAction(database, cfg => cfg.AddDocuments(cachedMappingXmls))))
    {
    }

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

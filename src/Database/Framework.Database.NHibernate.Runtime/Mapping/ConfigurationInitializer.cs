using NHibernate.Cfg;

namespace Framework.Database.NHibernate.Mapping;

public class ConfigurationInitializer(Action<Configuration> initAction) : IConfigurationInitializer
{
    public void Initialize(Configuration cfg) => initAction(cfg);

    public static IConfigurationInitializer Empty { get; } = new ConfigurationInitializer(_ => { });
}

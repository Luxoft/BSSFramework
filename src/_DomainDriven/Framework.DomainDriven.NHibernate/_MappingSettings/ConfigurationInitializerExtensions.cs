#nullable enable
using NHibernate.Cfg;

namespace Framework.DomainDriven.NHibernate;

public static class ConfigurationInitializerExtensions
{
    public static IConfigurationInitializer Add(this IConfigurationInitializer initializer, IConfigurationInitializer otherInitializer)
    {
        return new CompositeConfigurationInitializer([initializer, otherInitializer]);
    }

    private class CompositeConfigurationInitializer(IEnumerable<IConfigurationInitializer> initializers) : IConfigurationInitializer
    {
        public void Initialize(Configuration cfg)
        {
            foreach (var initializer in initializers)
            {
                initializer.Initialize(cfg);
            }
        }
    }
}

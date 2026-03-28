
namespace Framework.Database.NHibernate._MappingSettings;

public static class ConfigurationInitializerExtensions
{
    public static IConfigurationInitializer Add(this IConfigurationInitializer initializer, IConfigurationInitializer otherInitializer) =>
        new ConfigurationInitializer(
            cfg =>
            {
                initializer.Initialize(cfg);
                otherInitializer.Initialize(cfg);
            });
}

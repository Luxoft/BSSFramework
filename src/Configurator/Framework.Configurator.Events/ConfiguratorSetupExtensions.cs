using SecuritySystem.Configurator;

namespace Framework.Configurator;

public static class ConfiguratorSetupExtensions
{
    public static IConfiguratorSetup AddEvents(this IConfiguratorSetup setup) => setup.AddModule(new ConfiguratorEventModule());
}

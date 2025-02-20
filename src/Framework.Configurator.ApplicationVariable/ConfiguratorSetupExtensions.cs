namespace Framework.Configurator;

public static class ConfiguratorSetupExtensions
{
    public static IConfiguratorSetup AddApplicationVariables(this IConfiguratorSetup setup) =>
        setup.AddModule(new ConfiguratorApplicationVariableModule());
}

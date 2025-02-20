namespace Framework.Configurator;

public interface IConfiguratorSetup
{
    IConfiguratorSetup AddModule(IConfiguratorModule module);
}

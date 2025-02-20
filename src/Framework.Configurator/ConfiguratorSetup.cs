using Microsoft.Extensions.DependencyInjection;

namespace Framework.Configurator;

public class ConfiguratorSetup : IConfiguratorSetup
{
    private readonly List<IConfiguratorModule> modules = [];

    public IConfiguratorSetup AddModule(IConfiguratorModule module)
    {
        this.modules.Add(module);

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        this.modules.ForEach(module =>
                             {
                                 services.AddSingleton(module);
                                 module.AddServices(services);
                             });
    }
}

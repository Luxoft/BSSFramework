using Framework.Configurator.Handlers;
using Framework.Configurator.Interfaces;

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.Configurator;

namespace Framework.Configurator;

public class ConfiguratorApplicationVariableModule : IConfiguratorModule
{
    public string Name { get; } = "ApplicationVariables";

    public void AddServices(IServiceCollection services)
    {
        services.AddScoped<IGetSystemConstantsHandler, GetSystemConstantsHandler>();
        services.AddScoped<IUpdateSystemConstantHandler, UpdateSystemConstantHandler>();
    }

    public void MapApi(IEndpointRouteBuilder endpointsBuilder, string route)
    {
        endpointsBuilder.Get<IGetSystemConstantsHandler>($"{route}/api/constants")
                        .Post<IUpdateSystemConstantHandler>(route + "/api/constant/{name}");
    }
}

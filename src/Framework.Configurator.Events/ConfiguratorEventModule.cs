using Framework.Configurator.Handlers;
using Framework.Configurator.Interfaces;

using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Configurator;

public class ConfiguratorEventModule : IConfiguratorModule
{
    public string Name { get; } = "Events";

    public void AddServices(IServiceCollection services)
    {
        services.AddScoped<IGetDomainTypesHandler, GetDomainTypesHandler>();
        services.AddScoped<IForcePushEventHandler, ForcePushEventHandler>();
    }

    public void MapApi(IEndpointRouteBuilder endpointsBuilder, string route)
    {
        endpointsBuilder.Get<IGetDomainTypesHandler>($"{route}/api/domainTypes")
                        .Post<IForcePushEventHandler>(route + "/api/domainType/{domainTypeName}");
    }
}

using Framework.Authorization.BLL;
using Framework.Authorization.Generated.DTO;
using Framework.Configuration.BLL;
using Framework.Configuration.Generated.DTO;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.WebApiNetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterContextEvaluators(this IServiceCollection services)
    {
        services
            .AddScoped<IApiControllerBaseEvaluator<IAuthorizationBLLContext, IAuthorizationDTOMappingService>,
                ApiControllerBaseSingleCallEvaluator<IAuthorizationBLLContext, IAuthorizationDTOMappingService>>();

        services
            .AddScoped<IApiControllerBaseEvaluator<IConfigurationBLLContext, IConfigurationDTOMappingService>,
                ApiControllerBaseSingleCallEvaluator<IConfigurationBLLContext, IConfigurationDTOMappingService>>();

        return services;
    }
}

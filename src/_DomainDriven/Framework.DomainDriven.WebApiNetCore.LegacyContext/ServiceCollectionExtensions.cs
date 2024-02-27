using Framework.Authorization.BLL;
using Framework.Authorization.Generated.DTO;
using Framework.Configuration.BLL;
using Framework.Configuration.Generated.DTO;
using Framework.DomainDriven.ServiceModel.Service;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.WebApiNetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterContextEvaluators(this IServiceCollection services)
    {
        services
            .AddScoped<IApiControllerBaseEvaluator<EvaluatedData<IAuthorizationBLLContext, IAuthorizationDTOMappingService>>,
                ApiControllerBaseSingleCallEvaluator<
                EvaluatedData<IAuthorizationBLLContext, IAuthorizationDTOMappingService>>>();
        services
            .AddScoped<IApiControllerBaseEvaluator<EvaluatedData<IConfigurationBLLContext, IConfigurationDTOMappingService>>,
                ApiControllerBaseSingleCallEvaluator<
                EvaluatedData<IConfigurationBLLContext, IConfigurationDTOMappingService>>>();

        return services;
    }
}

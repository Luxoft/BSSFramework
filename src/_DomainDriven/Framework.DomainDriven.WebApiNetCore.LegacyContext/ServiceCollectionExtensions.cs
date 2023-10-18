using Framework.Authorization.BLL;
using Framework.Authorization.Generated.DTO;
using Framework.Configuration.BLL;
using Framework.Configuration.Generated.DTO;
using Framework.DependencyInjection;
using Framework.DomainDriven.ServiceModel.Service;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.WebApiNetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterContextEvaluators(this IServiceCollection services)
    {
        services.AddSingletonFrom<IServiceEvaluator<Framework.DomainDriven.BLL.Configuration.IConfigurationBLLContext>, IServiceEvaluator<IConfigurationBLLContext>>();

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

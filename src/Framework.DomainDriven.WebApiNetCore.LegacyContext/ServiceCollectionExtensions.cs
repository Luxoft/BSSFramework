using Framework.Authorization.BLL;
using Framework.Authorization.Generated.DTO;
using Framework.Configuration.BLL;
using Framework.Configuration.Generated.DTO;
using Framework.DependencyInjection;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore.Integration;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.WebApiNetCore;

public static class ServiceCollectionExtensions
{
    private static IServiceCollection RegisterContextEvaluators(this IServiceCollection services)
    {
        services.AddSingleton<IContextEvaluator<IAuthorizationBLLContext>, ContextEvaluator<IAuthorizationBLLContext>>();
        services.AddSingleton<IContextEvaluator<IConfigurationBLLContext>, ContextEvaluator<IConfigurationBLLContext>>();
        services
                .AddSingletonFrom<IContextEvaluator<Framework.DomainDriven.BLL.Configuration.IConfigurationBLLContext>,
                        IContextEvaluator<IConfigurationBLLContext>>();

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

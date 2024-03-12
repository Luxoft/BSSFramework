using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.WebApiNetCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterContextEvaluators(this IServiceCollection services, bool useSingleCall = true)
    {
        services.AddScoped(
            typeof(IApiControllerBaseEvaluator<,>),
            useSingleCall ? typeof(ApiControllerBaseSingleCallEvaluator<,>) : typeof(ApiControllerNewScopeEvaluator<,>));

        return services;
    }
}

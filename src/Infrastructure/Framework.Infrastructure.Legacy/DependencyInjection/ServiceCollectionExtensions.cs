using Framework.Infrastructure.ApiControllerBaseEvaluator;
using Framework.Infrastructure.ContextEvaluator;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterContextEvaluators(this IServiceCollection services, bool useSingleCall = false)
    {
        services.AddScoped(
            typeof(IApiControllerBaseEvaluator<,>),
            useSingleCall ? typeof(ApiControllerBaseSingleCallEvaluator<,>) : typeof(ApiControllerNewScopeEvaluator<,>));

        if (!useSingleCall)
        {
            services.AddSingleton(typeof(IContextEvaluator<,>), typeof(ContextEvaluator<,>));
        }

        return services;
    }
}

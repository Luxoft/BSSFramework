using CommonFramework.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBssFramework(this IServiceCollection services, Action<IBssFrameworkBuilder> setupAction) =>

        services.Initialize<BssFrameworkBuilder>(setupAction);
}

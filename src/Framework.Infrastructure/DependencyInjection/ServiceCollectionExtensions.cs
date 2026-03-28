using CommonFramework.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddBssFramework(Action<IBssFrameworkBuilder> setupAction) =>

            services.Initialize<BssFrameworkBuilder>(setupAction);
    }
}

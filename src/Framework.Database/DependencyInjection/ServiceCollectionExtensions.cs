using CommonFramework.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.DependencyInjection;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddGenericDatabaseServices()
        {
            services.AddSingleton<IInitializeManager, InitializeManager>();

            return services;
        }

        public IServiceCollection RegisterListeners(Action<IDALListenerBuilder> setupAction) =>
            services.Initialize<DALListenerBuilder>(setupAction);
    }
}

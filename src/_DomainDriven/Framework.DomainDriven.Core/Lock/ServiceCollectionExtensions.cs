using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Lock;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterNamedLocks(this IServiceCollection services, Action<IGenericNamedLockSetup> setupAction)
    {
        var setup = new GenericNamedLockSetup();

        setupAction(setup);

        setup.Initialize(services);

        return services;
    }
}

using CommonFramework;

using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddScopedFromLazyInterfaceImplement<TServiceInterface, TServiceImplementation>(
        this IServiceCollection services,
        bool registerImpl = true)
        where TServiceImplementation : class, TServiceInterface
        where TServiceInterface : class
    {
        return services.Pipe(registerImpl, s => s.AddScoped<TServiceImplementation>())
                       .AddScoped(
                           sp => LazyInterfaceImplementHelper.CreateProxy<TServiceInterface>(
                               sp.GetRequiredService<TServiceImplementation>));
    }

    public static IServiceCollection AddScopedFromLazyObject<TService, TServiceImplementation>(
        this IServiceCollection services,
        bool registerImpl = true)
        where TServiceImplementation : class, TService
        where TService : class
    {
        return services.Pipe(registerImpl, s => s.AddScoped<TServiceImplementation>())
                       .AddScoped<ILazyObject<TService>>(sp => new LazyObject<TService>(sp.GetRequiredService<TServiceImplementation>))
                       .AddScoped(sp => sp.GetRequiredService<ILazyObject<TService>>().Value);
    }

    public static IServiceCollection AddNotImplemented<TService>(this IServiceCollection services, string? message = null, bool isScoped = false)
        where TService : class
    {
        if (isScoped)
        {
            return services.AddScoped(typeof(TService), _ => LazyInterfaceImplementHelper.CreateNotImplemented<TService>(message));
        }
        else
        {
            return services.AddSingleton(typeof(TService), _ => LazyInterfaceImplementHelper.CreateNotImplemented<TService>(message));
        }
    }
}

﻿using Framework.Core;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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

    public static IServiceCollection AddScopedFrom<TService, TServiceImplementation>(this IServiceCollection services)
        where TServiceImplementation : class, TService
        where TService : class
    {
        return services.AddScopedFrom<TService, TServiceImplementation>(v => v);
    }

    public static IServiceCollection AddScopedFrom<TService, TSource>(this IServiceCollection services, Func<TSource, TService> selector)
        where TService : class
    {
        return services.AddScoped(sp => selector(sp.GetRequiredService<TSource>()));
    }

    public static IServiceCollection AddSingletonFrom<TService, TServiceImplementation>(this IServiceCollection services)
        where TServiceImplementation : class, TService
        where TService : class
    {
        return services.AddSingletonFrom<TService, TServiceImplementation>(v => v);
    }

    public static IServiceCollection AddSingletonFrom<TService, TSource>(this IServiceCollection services, Func<TSource, TService> selector)
        where TService : class
    {
        return services.AddSingleton(sp => selector(sp.GetRequiredService<TSource>()));
    }

    public static IServiceCollection ReplaceScoped<TService, TServiceImplementation>(this IServiceCollection services)
        where TServiceImplementation : class, TService
        where TService : class
    {
        return services.Replace(ServiceDescriptor.Scoped<TService, TServiceImplementation>());
    }

    public static IServiceCollection ReplaceScopedFrom<TService, TServiceImplementation>(this IServiceCollection services)
        where TServiceImplementation : class, TService
        where TService : class
    {
        return services.Replace(ServiceDescriptor.Scoped<TService>(sp => sp.GetRequiredService<TServiceImplementation>()));
    }

    public static IServiceCollection ReplaceSingleton<TService, TServiceImplementation>(this IServiceCollection services)
        where TServiceImplementation : class, TService
        where TService : class
    {
        return services.Replace(ServiceDescriptor.Singleton<TService, TServiceImplementation>());
    }

    public static IServiceCollection ReplaceSingletonFrom<TService, TServiceImplementation>(this IServiceCollection services)
        where TServiceImplementation : class, TService
        where TService : class
    {
        return services.Replace(ServiceDescriptor.Singleton<TService>(sp => sp.GetRequiredService<TServiceImplementation>()));
    }

    public static IServiceCollection AddNotImplemented<TService>(this IServiceCollection services, string message = null)
        where TService : class
    {
        return services.AddSingleton(typeof(TService), _ => LazyInterfaceImplementHelper.CreateNotImplemented<TService>(message));
    }
}

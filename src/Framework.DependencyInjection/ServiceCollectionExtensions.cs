using System;

using Framework.Core;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Framework.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddScopedFromLazyInterfaceImplement<TInterface, TImplementation>(this IServiceCollection services)
        where TImplementation : class, TInterface
        where TInterface : class
    {
        return services.AddScoped<TImplementation>()
                       .AddScoped(sp => LazyInterfaceImplementHelper.CreateProxy<TInterface>(() => sp.GetRequiredService<TImplementation>()));
    }

    public static IServiceCollection AddScopedFromLazy<TInterface, TImplementation>(this IServiceCollection services)
            where TImplementation : class, TInterface
            where TInterface : class
    {
        return services.AddScoped<TImplementation>()
                       .AddScoped(sp => new Lazy<TInterface>(sp.GetRequiredService<TImplementation>))
                       .AddScoped(sp => sp.GetRequiredService<Lazy<TInterface>>().Value);
    }

    public static IServiceCollection AddScopedFrom<TSource, TImplementation>(this IServiceCollection services)
            where TImplementation : class, TSource
            where TSource : class
    {
        return services.AddScoped<TSource>(sp => sp.GetRequiredService<TImplementation>());
    }

    public static IServiceCollection AddSingletonFrom<TSource, TImplementation>(this IServiceCollection services)
            where TImplementation : class, TSource
            where TSource : class
    {
        return services.AddSingleton<TSource>(sp => sp.GetRequiredService<TImplementation>());
    }

    public static IServiceCollection ReplaceScoped<TSource, TImplementation>(this IServiceCollection services)
            where TImplementation : class, TSource
            where TSource : class
    {
        return services.Replace(ServiceDescriptor.Scoped<TSource, TImplementation>());
    }

    public static IServiceCollection ReplaceScopedFrom<TSource, TImplementation>(this IServiceCollection services)
            where TImplementation : class, TSource
            where TSource : class
    {
        return services.Replace(ServiceDescriptor.Scoped<TSource>(sp => sp.GetRequiredService<TImplementation>()));
    }

    public static IServiceCollection ReplaceSingleton<TSource, TImplementation>(this IServiceCollection services)
            where TImplementation : class, TSource
            where TSource : class
    {
        return services.Replace(ServiceDescriptor.Singleton<TSource, TImplementation>());
    }

    public static IServiceCollection ReplaceSingletonFrom<TSource, TImplementation>(this IServiceCollection services)
            where TImplementation : class, TSource
            where TSource : class
    {
        return services.Replace(ServiceDescriptor.Singleton<TSource>(sp => sp.GetRequiredService<TImplementation>()));
    }
}

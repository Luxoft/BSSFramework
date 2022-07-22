using System;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLL;

public static class ServiceProviderExtensions
{
    /// <summary>
    /// Попытка закрыть текущую бд-сессию (если она существует) и вызывать механизм сброса евентов в DALListener-ы
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void TryCloseDbSession([NotNull] this IServiceProvider serviceProvider)
    {
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        var lazyDbSession = serviceProvider.GetRequiredService<Lazy<IDBSession>>();

        if (lazyDbSession.IsValueCreated)
        {
            lazyDbSession.Value.Close();
        }
    }

    /// <summary>
    /// Попытка промаркировать текущую бд-сессию (если она существует), что случилась ошибка и ничего в базу записывать не нужно.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void TryFaultDbSession([NotNull] this IServiceProvider serviceProvider)
    {
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        var lazyDbSession = serviceProvider.GetRequiredService<Lazy<IDBSession>>();

        if (lazyDbSession.IsValueCreated)
        {
            lazyDbSession.Value.AsFault();
        }
    }
}

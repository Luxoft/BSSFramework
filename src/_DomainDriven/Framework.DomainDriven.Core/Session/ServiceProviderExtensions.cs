using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven;

public static class ServiceProviderExtensions
{
    /// <summary>
    /// Попытка закрыть текущую бд-сессию (если она существует) и вызывать механизм сброса евентов в DALListener-ы
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static async Task TryCloseDbSessionAsync(this IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        var lazyDbSession = serviceProvider.GetRequiredService<ILazyObject<IDBSession>>();

        if (lazyDbSession.IsValueCreated)
        {
            await lazyDbSession.Value.CloseAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Попытка промаркировать текущую бд-сессию (если она существует), что случилась ошибка и ничего в базу записывать не нужно.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void TryFaultDbSession(this IServiceProvider serviceProvider)
    {
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        var lazyDbSession = serviceProvider.GetRequiredService<ILazyObject<IDBSession>>();

        if (lazyDbSession.IsValueCreated)
        {
            lazyDbSession.Value.AsFault();
        }
    }
}

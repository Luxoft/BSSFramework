using System;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLL;

public static class ServiceProviderExtensions
{
    public static void TryCloseDbSession([NotNull] this IServiceProvider serviceProvider)
    {
        if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

        var lazyDbSession = serviceProvider.GetRequiredService<Lazy<IDBSession>>();

        if (lazyDbSession.IsValueCreated)
        {
            lazyDbSession.Value.Close();
        }
    }
}

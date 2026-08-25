using Anch.DependencyInjection;

using Framework.Infrastructure.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Notification.DependencyInjection;

public static class ServiceCollectionExtensions
{
    extension<TSelf>(IBssFrameworkSetup<TSelf> setup)
        where TSelf : IBssFrameworkSetup<TSelf>
    {
        public TSelf AddNotification(IConfiguration configuration, Action<INotificationSetup>? setupAction = null) =>
            setup.AddServices(sc => sc.AddNotification(configuration, setupAction));
    }

    extension(IServiceCollection services)
    {
        public void AddNotification(IConfiguration configuration, Action<INotificationSetup>? setupAction = null)
            => services.Initialize<IServiceCollection, NotificationSetup>(new NotificationSetup(configuration), setupAction);
    }
}

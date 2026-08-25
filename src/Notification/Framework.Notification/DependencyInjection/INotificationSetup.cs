using Framework.Core;

namespace Framework.Notification.DependencyInjection;

public interface INotificationSetup
{
    INotificationSetup SetSender<TSender>()
        where TSender : class, IMessageSender<Notification.Domain.Notification>;

    INotificationSetup IsProduction(bool value);
}

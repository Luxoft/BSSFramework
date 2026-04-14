using Framework.Application.Repository;
using Framework.Configuration.Domain;
using Framework.Notification;

namespace Framework.Configuration.BLL;

public class ConfigurationSentNotificationLogger(IRepositoryFactory<SentMessage> sentMessageRepositoryFactory) : ISentNotificationLogger
{
    public Task LogAsync(Framework.Notification.Domain.Notification notification, CancellationToken cancellationToken) =>

        sentMessageRepositoryFactory.Create().SaveAsync(notification.ToSentMessage(), cancellationToken);
}

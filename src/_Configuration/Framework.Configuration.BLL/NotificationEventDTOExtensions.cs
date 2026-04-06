using Framework.Notification.DTO;
using Framework.Configuration.Domain;
using Framework.Notification.Domain;

namespace Framework.Configuration.BLL;

public static class NotificationEventDTOExtensions
{
    public static SentMessage ToSentMessage(this NotificationEventDTO notification)
    {
        if (notification == null) throw new ArgumentNullException(nameof(notification));

        return new SentMessage(
            notification.From,
            string.Join(",", notification.Targets.Where(z => z.Type == RecipientRole.To).Select(z => z.Name)),
            notification.Subject,
            notification.Message.Message,
            "",
            string.Empty,
            string.Join(",", notification.Targets.Where(z => z.Type == RecipientRole.Copy).Select(z => z.Name)),
            "",
            null,
            string.Join(",", notification.Targets.Where(z => z.Type == RecipientRole.ReplyTo).Select(z => z.Name)));
    }
}

using Framework.Notification.DTO;
using Framework.Notification.New;
using Framework.Configuration.Domain;

using Attachment = Framework.Notification.New.Attachment;

namespace Framework.Configuration.BLL;

public static class NotificationEventDTOExtensions
{
    public static SentMessage ToSentMessage(this NotificationEventDTO notification)
    {
        if (notification == null) throw new ArgumentNullException(nameof(notification));

        return new SentMessage(
                               notification.From,
                               string.Join(",", notification.Targets.Where(z => z.Type == ReceiverRole.To).Select(z => z.Name)),
                               notification.Subject,
                               notification.Message.Message,
                               notification.TechnicalInformation.MessageTemplateCode,
                               string.Empty,
                               string.Join(",", notification.Targets.Where(z => z.Type == ReceiverRole.Copy).Select(z => z.Name)),
                               notification.TechnicalInformation.ContextObjectType,
                               notification.TechnicalInformation.ContextObjectId,
                               string.Join(",", notification.Targets.Where(z => z.Type == ReceiverRole.ReplyTo).Select(z => z.Name)));
    }

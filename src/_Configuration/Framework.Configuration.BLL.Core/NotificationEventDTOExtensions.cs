using System;
using System.Linq;

using Framework.Notification.DTO;
using Framework.Notification.New;
using Framework.Configuration.Domain;

using Attachment = Framework.Notification.New.Attachment;

namespace Framework.Configuration.BLL
{
    public static class NotificationEventDTOExtensions
    {
        public static SentMessage ToSentMessage(this NotificationEventDTO notification)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));

            return new SentMessage(
                notification.From,
                string.Join(",", notification.Targets.Where(z => z.Type == NotificationTargetTypes.To).Select(z => z.Name)),
                notification.Subject,
                notification.Message.Message,
                notification.TechnicalInformation.MessageTemplateCode,
                string.Empty,
                string.Join(",", notification.Targets.Where(z => z.Type == NotificationTargetTypes.Copy).Select(z => z.Name)),
                notification.TechnicalInformation.ContextObjectType,
                notification.TechnicalInformation.ContextObjectId,
                string.Join(",", notification.Targets.Where(z => z.Type == NotificationTargetTypes.ReplyTo).Select(z => z.Name)));
        }

        public static Message ToMessage(this NotificationEventDTO notification)
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));


            return new Message(
                  notification.From,
                  notification.Targets.Select(t => new ReceiverAddress(t.Name, (ReceiverRole)t.Type)),
                  notification.Subject,
                  notification.Message.Message,
                  notification.Message.IsBodyHtml,
                  notification.Attachments.Select(a => ToAttachment(a)));
        }

        private static Attachment ToAttachment(NotificationAttachmentDTO notificationAttachment)
        {
            return new Attachment(notificationAttachment.Content, notificationAttachment.Name)
                   {
                       ContentId = notificationAttachment.ContentId,
                       IsInline = notificationAttachment.IsInline
                   };
        }
    }
}

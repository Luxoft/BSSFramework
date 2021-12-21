using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Core.Serialization;
using Framework.DomainDriven.BLL;
using Framework.Notification;
using Framework.Notification.DTO;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL.Notification
{
    /// <summary>
    /// Конвертор нотофикации в DTO
    /// </summary>
    public class MessageTemplateDTOFormatter : BLLContextContainer<IConfigurationBLLContext>, IFormatter<MessageTemplateNotification, NotificationEventDTO>
    {
        private readonly MailAddress defaultSender;

        public MessageTemplateDTOFormatter(IConfigurationBLLContext context, MailAddress defaultSender)
                : base(context)
        {
            this.defaultSender = defaultSender ?? throw new ArgumentNullException(nameof(defaultSender));
        }


        public NotificationEventDTO Format(MessageTemplateNotification message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var notification = this.CreateNotification(message);

            notification.Message.IsBodyHtml = true;

            return new NotificationEventDTO(notification);
        }

        private static IEnumerable<string> SplitMails([NotNull] IEnumerable<string> receivers)
        {
            if (receivers == null) { throw new ArgumentNullException(nameof(receivers)); }

            return receivers.SelectMany(z => z.Split(new[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries));
        }

        private Framework.Notification.Notification CreateNotification(MessageTemplateNotification message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var messageTemplateBLL = this.Context.Logics.MessageTemplate;

            var messageTemplate = messageTemplateBLL.GetByCode(message.MessageTemplateCode) ?? new MessageTemplate();

            var includeAttachments = message.Subscription.Maybe(s => s.IncludeAttachments, true);

            var sender = message.Subscription.Maybe(s => s.Sender) ?? this.defaultSender;

            var mailMessage = messageTemplateBLL.CreateMailMessage(
                message,
                messageTemplate,
                includeAttachments,
                message.ContextObject,
                sender,
                SplitMails(message.Receivers),
                SplitMails(message.CopyReceivers),
                SplitMails(message.ReplyTo),
                message.Attachments);

            var technicalInformation = new NotificationTechnicalInformation(
                message.MessageTemplateCode,
                message.ContextObjectType.Name,
                (message.ContextObject as IIdentityObject<Guid>).MaybeToNullable(obj => obj.Id));

            return new Framework.Notification.Notification(technicalInformation, mailMessage);
        }
    }
}

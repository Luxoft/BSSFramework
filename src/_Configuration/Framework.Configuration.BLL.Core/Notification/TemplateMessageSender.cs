using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

using Framework.Configuration.Core;

using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Notification;
using Framework.Notification.DTO;
using Framework.Persistent;

using Serilog;

namespace Framework.Configuration.BLL.Notification
{
    public static partial class MessageSenderExtensions
    {
        public static IMessageSender<MessageTemplateNotification> ToMessageTemplateSender(this IMessageSender<NotificationEventDTO> notificationEventSender, IConfigurationBLLContext context, MailAddress defaultSender)
        {
            if (notificationEventSender == null)
            {
                throw new ArgumentNullException(nameof(notificationEventSender));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (defaultSender == null)
            {
                throw new ArgumentNullException(nameof(defaultSender));
            }

            return new TemplateMessageSender(context, notificationEventSender, defaultSender);
        }

        public static IMessageSender<Exception> ToExceptionSender(this IMessageSender<Framework.Notification.New.Message> messageSender, IConfigurationBLLContext context, MailAddress sender, IEnumerable<string> receivers)
        {
            if (messageSender == null)
            {
                throw new ArgumentNullException(nameof(messageSender));
            }

            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (receivers == null)
            {
                throw new ArgumentNullException(nameof(receivers));
            }

            return new ExceptionMessageSender(context, messageSender, sender, receivers);
        }

        private class TemplateMessageSender : BLLContextContainer<IConfigurationBLLContext>, IMessageSender<MessageTemplateNotification>
        {
            private readonly MailAddress _defaultSender;
            private readonly IMessageSender<NotificationEventDTO> _notificationEventSender;

            public TemplateMessageSender(IConfigurationBLLContext context, IMessageSender<NotificationEventDTO> notificationEventSender, MailAddress defaultSender)
                : base(context)
            {
                if (defaultSender == null)
                {
                    throw new ArgumentNullException(nameof(defaultSender));
                }

                if (notificationEventSender == null)
                {
                    throw new ArgumentNullException(nameof(notificationEventSender));
                }

                this._defaultSender = defaultSender;
                this._notificationEventSender = notificationEventSender;
                this.Logger = Log.Logger.ForContext(this.GetType());
            }

            private ILogger Logger { get; }

            public void Send(MessageTemplateNotification message, TransactionMessageMode transactionMessageMode)
            {
                if (message == null)
                {
                    throw new ArgumentNullException(nameof(message));
                }

                if (!message.SendWithEmptyListOfRecipients)
                {
                    var receiversCollection = message.Receivers.ToList();

                    if (!receiversCollection.Any())
                    {
                        return;
                    }
                }

                var notification = this.CreateNotification(message);

                notification.Message.IsBodyHtml = true;

                this.Logger.Information(
                                        "Send message template: '{MessageTemplateCode}'; Receivers: '{To}'; From: '{From}'; Send message body:{Body}",
                                        message.MessageTemplateCode,
                                        notification.Message.To,
                                        notification.Message.From,
                                        notification.Message.Body);

                this._notificationEventSender.Send(new NotificationEventDTO(notification), transactionMessageMode);
            }

            private Framework.Notification.Notification CreateNotification(MessageTemplateNotification message)
            {
                if (message == null)
                {
                    throw new ArgumentNullException(nameof(message));
                }

                var messageTemplate = new MessageTemplate();

                var splittedReceivers = message.Receivers.SelectMany(z => z.Split(new[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToList();
                var splittedCarbonCopy = message.CopyReceivers.SelectMany(z => z.Split(new[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToList();
                var splittedReplyTo = message.ReplyTo.SelectMany(z => z.Split(new[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToList();

                var includeAttachments = message.Subscription.Maybe(s => s.IncludeAttachments, true);

                var sender = message.Subscription.Maybe(s => s.Sender) ?? this._defaultSender;

                var messageTemplateBLL = new MessageTemplateBLL(this.Context);

                var mailMessage = messageTemplateBLL.CreateMailMessage(
                    message,
                    messageTemplate,
                    includeAttachments,
                    message.ContextObject,
                    sender,
                    splittedReceivers,
                    splittedCarbonCopy,
                    splittedReplyTo,
                    message.Attachments);

                var technicalInformation = new NotificationTechnicalInformation(
                    message.MessageTemplateCode,
                    message.ContextObjectType.Name,
                    (message.ContextObject as IIdentityObject<Guid> ?? (message.ContextObject as IDomainObjectVersions).Maybe(ver => ver.Current ?? ver.Previous) as IIdentityObject<Guid>).MaybeToNullable(obj => obj.Id));

                return new Framework.Notification.Notification(technicalInformation, mailMessage);
            }
        }
    }
}

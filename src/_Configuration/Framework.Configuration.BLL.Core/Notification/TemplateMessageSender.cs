using Framework.Configuration.Core;

using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Notification;
using Framework.Notification.DTO;
using Framework.Persistent;

using CommonFramework;

namespace Framework.Configuration.BLL.Notification;

public class TemplateMessageSender(
    IConfigurationBLLContext context,
    IMessageSender<NotificationEventDTO> notificationEventSender,
    IDefaultMailSenderContainer defaultMailSenderContainer)
    : IMessageSender<MessageTemplateNotification>
{
    public async Task SendAsync(MessageTemplateNotification message, CancellationToken cancellationToken)
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

        await notificationEventSender.SendAsync(new NotificationEventDTO(notification), cancellationToken);
    }

    private Framework.Notification.Notification CreateNotification(MessageTemplateNotification message)
    {
        var messageTemplate = new MessageTemplate();

        var splittedReceivers = message.Receivers.SelectMany(z => z.Split(new[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToList();
        var splittedCarbonCopy = message.CopyReceivers.SelectMany(z => z.Split(new[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToList();
        var splittedReplyTo = message.ReplyTo.SelectMany(z => z.Split(new[] { ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)).ToList();

        var includeAttachments = message.Subscription.Maybe(s => s.IncludeAttachments, true);

        var sender = message.Subscription.Maybe(s => s.Sender) ?? defaultMailSenderContainer.DefaultSender;

        var mailMessage = new MessageTemplateBLL(context).CreateMailMessage(
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

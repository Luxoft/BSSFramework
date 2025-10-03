using Framework.Configuration.Core;

using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Notification;
using Framework.Notification.DTO;
using Framework.Persistent;

using CommonFramework;

namespace Framework.Configuration.BLL.Notification;

public class TemplateMessageSender : BLLContextContainer<IConfigurationBLLContext>, IMessageSender<MessageTemplateNotification>
{
    private readonly IDefaultMailSenderContainer defaultMailSenderContainer;
    private readonly IMessageSender<NotificationEventDTO> notificationEventSender;

    public TemplateMessageSender(IConfigurationBLLContext context, IMessageSender<NotificationEventDTO> notificationEventSender, IDefaultMailSenderContainer defaultMailSenderContainer)
            : base(context)
    {
        this.defaultMailSenderContainer = defaultMailSenderContainer ?? throw new ArgumentNullException(nameof(defaultMailSenderContainer));
        this.notificationEventSender = notificationEventSender ?? throw new ArgumentNullException(nameof(notificationEventSender));
    }

    public void Send(MessageTemplateNotification message)
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

        this.notificationEventSender.Send(new NotificationEventDTO(notification));
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

        var sender = message.Subscription.Maybe(s => s.Sender) ?? this.defaultMailSenderContainer.DefaultSender;

        var mailMessage = new MessageTemplateBLL(this.Context).CreateMailMessage(
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

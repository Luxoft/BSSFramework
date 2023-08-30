using System.Net.Mail;
using Framework.Core;
using Attachment = Framework.Notification.New.Attachment;

namespace Framework.Notification;

public static class MessageTemplateNotificationSenderExtensions
{
    public static void Send<T>(this IMessageSender<MessageTemplateNotification> sender, string messageTemplateCode, T contextObject, IEnumerable<string> receivers, ISubscription subscription = null)
    {
        if (sender == null) throw new ArgumentNullException(nameof(sender));
        if (messageTemplateCode == null) throw new ArgumentNullException(nameof(messageTemplateCode));
        if (contextObject == null) throw new ArgumentNullException(nameof(contextObject));
        if (receivers == null) throw new ArgumentNullException(nameof(receivers));

        sender.Send(new MessageTemplateNotification(messageTemplateCode, contextObject, typeof(T), receivers,  Enumerable.Empty<System.Net.Mail.Attachment>(), subscription));
    }

    /// <summary>
    /// Creates and sends e-mail message according to input params
    /// </summary>
    /// <param name="sender">Notification message sender</param>
    /// <param name="subject">E-mail subject</param>
    /// <param name="body">E-mail body</param>
    /// <param name="recipients">A list of recipients</param>
    /// <param name="attachments">A list of attachments</param>
    /// <param name="from">E-mail sender</param>
    /// <param name="messageTemplateCode">Message Template Code (if not specified default value is 'auto-generated')</param>
    public static void Send(this IMessageSender<Notification> sender, string subject, string body, IEnumerable<string> recipients, IEnumerable<Attachment> attachments, string from = null, string messageTemplateCode = "auto-generated")
    {
        if (sender == null) throw new ArgumentNullException(nameof(sender));
        if (subject == null) throw new ArgumentNullException(nameof(subject));
        if (recipients == null) throw new ArgumentNullException(nameof(recipients));

        var notification = GetNotification(subject, body, recipients, attachments, from, messageTemplateCode);

        sender.Send(notification);
    }

    private static Notification GetNotification(string subject, string body, IEnumerable<string> recipients, IEnumerable<Attachment> attachments, string sender, string messageTemplateCode)
    {
        var mailMessage = new MailMessage
                          {
                                  From = new MailAddress(sender),
                                  Body = body,
                                  IsBodyHtml = body.StartsWith("<html")
                          };

        mailMessage.Attachments.AddRange(attachments.Select(x => new System.Net.Mail.Attachment(new MemoryStream(x.Data.ToArray()), x.Filename)));
        mailMessage.Subject = subject;
        mailMessage.To.AddRange(recipients.ToList(x => new MailAddress(x)));
        return new Notification(new NotificationTechnicalInformation(messageTemplateCode, string.Empty, null), mailMessage);
    }
}

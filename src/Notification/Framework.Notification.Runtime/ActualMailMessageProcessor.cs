using System.Net.Mail;

using Anch.Core;

using Framework.Notification.Domain;
using Framework.Notification.MailMessageModifier;

namespace Framework.Notification;

public class ActualMailMessageProcessor(IEnumerable<IMailMessageModifier> mailMessageModifiers) : IActualMailMessageProcessor
{
    public MailMessage GetActualMailMessage(MailMessage baseMessage)
    {
        var newMailMessage = baseMessage.Clone();

        mailMessageModifiers.Foreach(m => m.Modify(newMailMessage));

        return newMailMessage;
    }
}

using System.Net.Mail;

using Anch.Core;

using Framework.Notification.Domain;
using Framework.Notification.MailMessageModifier;

namespace Framework.Notification;

public class ActualMailMessageProcessor(IEnumerable<IMailMessageModifier> mailMessageModifiers) : IActualMailMessageProcessor
{
    /// <summary>
    /// Applies all <see cref="IMailMessageModifier"/>s to a CLONE of the message (the original is kept intact, e.g. for logging).
    /// Modifiers run in DI registration order and each sees the result of the previous one, so the order is semantically significant
    /// (e.g. redirect-to-test must come before anything that relies on the final recipients).
    /// </summary>
    public MailMessage GetActualMailMessage(MailMessage baseMessage)
    {
        var newMailMessage = baseMessage.Clone();

        mailMessageModifiers.Foreach(m => m.Modify(newMailMessage));

        return newMailMessage;
    }
}

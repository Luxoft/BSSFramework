using System.Net.Mail;

using Anch.Core;

using Framework.Notification.Domain;
using Framework.Notification.Settings;

using Microsoft.Extensions.Options;

namespace Framework.Notification.MailMessageModifier;

public class RewriteReceiversMailMessageModifier(IOptions<RewriteReceiversSettings> settings) : IMailMessageModifier
{
    /// <summary>
    /// Replaces recipient addresses according to the rewrite rules. If at least one rule is configured, this is an inner join:
    /// recipients without a matching rule (or matching a rule with empty <c>To</c>) are DROPPED, not left unchanged.
    /// One recipient matching several rules is expanded into several recipients. With no rules the message is untouched.
    /// </summary>
    public void Modify(MailMessage message)
    {
        if (settings.Value.RewriteRules.Any())
        {
            message.Recipients =
            [
                .. message.Recipients.Join(
                              settings.Value.RewriteRules.Where(rr => rr.To.Any()),
                              mar => mar.Address.Address,
                              rr => rr.From,
                              (mar, rr) => RecipientsHelper.ToRecipients(rr.To).Select(newAddress => mar with { Address = newAddress }))
                          .SelectMany()
            ];
        }
    }
}

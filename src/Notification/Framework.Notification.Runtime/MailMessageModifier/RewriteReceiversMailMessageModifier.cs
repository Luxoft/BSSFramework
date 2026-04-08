using System.Net.Mail;

using CommonFramework;

using Framework.Notification.Domain;
using Framework.Notification.Settings;

using Microsoft.Extensions.Options;

namespace Framework.Notification.MailMessageModifier;

public class RewriteReceiversMailMessageModifier(IOptions<RewriteReceiversSettings> settings) : IMailMessageModifier
{
    public void Modify(MailMessage message) =>

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

using System.Net.Mail;

using CommonFramework;

using Framework.Notification.Domain;
using Framework.Notification.Extensions;
using Framework.Notification.Settings;

using Microsoft.Extensions.Options;

namespace Framework.Notification;

public class RewriteReceiversService(IOptions<RewriteReceiversSettings> settings) : IRewriteReceiversService
{
    public void RewriteRecipients(MailMessage message) =>

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

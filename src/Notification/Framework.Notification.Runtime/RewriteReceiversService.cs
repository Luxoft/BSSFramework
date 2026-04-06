using System.Net.Mail;

using CommonFramework;
using Framework.Notification.Domain;
using Framework.Notification.DTO;
using Framework.Notification.Extensions;
using Framework.Notification.Settings;

using Microsoft.Extensions.Options;

namespace Framework.Notification;

public class RewriteReceiversService(IOptionsSnapshot<RewriteReceiversSettings> settings) : IRewriteReceiversService
{
    public virtual void RewriteToRecipients(MailMessage message, NotificationEventDTO dto)
    {
        var recipients = this.GetRecipients(dto, RecipientRole.To);
        message.To.Clear();
        message.To.AddRange(recipients);
    }

    public virtual void RewriteCopyRecipients(MailMessage message, NotificationEventDTO dto)
    {
        var recipients = this.GetRecipients(dto, RecipientRole.Copy);
        message.CC.Clear();
        message.CC.AddRange(recipients);
    }

    public void RewriteReplyTo(MailMessage message, NotificationEventDTO dto)
    {
        var recipients = this.GetRecipients(dto, RecipientRole.ReplyTo);
        message.ReplyToList.Clear();
        message.ReplyToList.AddRange(recipients);
    }

    private IEnumerable<MailAddress> GetRecipients(NotificationEventDTO dto, RecipientRole type) => dto.Targets
            .Where(z => z.Type == type)
            .Select(z =>
                    {
                        var newRecipients = settings.Value.RewriteRules
                                                .Where(x => string.Compare(x.From, z.Name, StringComparison.InvariantCultureIgnoreCase) == 0 && x.To.Any())
                                                .SelectMany(x => RecipientsHelper.ToRecipients(x.To));

                        return newRecipients.Any()
                                       ? newRecipients
                                       : [new MailAddress(z.Name)];
                    })
            .SelectMany(z => z);
}

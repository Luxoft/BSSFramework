using System.Net.Mail;

using CommonFramework;

using Framework.Notification.DTO;
using Framework.NotificationCore.Extensions;
using Framework.NotificationCore.Settings;

using Microsoft.Extensions.Options;

namespace Framework.NotificationCore.Services;

public class RewriteReceiversService : IRewriteReceiversService
{
    private readonly RewriteReceiversSettings settings;

    public RewriteReceiversService(IOptions<RewriteReceiversSettings> settings)
    {
        this.settings = settings.Value;
    }

    public virtual void RewriteToRecipients(MailMessage message, NotificationEventDTO dto)
    {
        var recepients = this.GetRecipients(dto, NotificationTargetTypes.To);
        message.To.Clear();
        message.To.AddRange(recepients);
    }

    public virtual void RewriteCopyRecipients(MailMessage message, NotificationEventDTO dto)
    {
        var recepients = this.GetRecipients(dto, NotificationTargetTypes.Copy);
        message.CC.Clear();
        message.CC.AddRange(recepients);
    }

    public void RewriteReplyTo(MailMessage message, NotificationEventDTO dto)
    {
        var recepients = this.GetRecipients(dto, NotificationTargetTypes.ReplyTo);
        message.ReplyToList.Clear();
        message.ReplyToList.AddRange(recepients);
    }

    private IEnumerable<MailAddress> GetRecipients(NotificationEventDTO dto, NotificationTargetTypes type) => dto.Targets
            .Where(z => z.Type == type)
            .Select(z =>
                    {
                        var newRecipients = this.settings.RewriteRules
                                                .Where(x => string.Compare(x.From, z.Name, StringComparison.InvariantCultureIgnoreCase) == 0 && x.To.Any())
                                                .SelectMany(x => RecipientsHelper.ToRecipients(x.To));

                        return newRecipients.Any()
                                       ? newRecipients
                                       : new[] { new MailAddress(z.Name) };
                    })
            .SelectMany(z => z);
}

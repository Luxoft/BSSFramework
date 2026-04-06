using System.Net.Mail;

using Framework.Notification.Domain;
using Framework.Notification.DTO;
using Framework.Notification.Settings;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Framework.Notification
{
    public class ProdSmtpMessageSender(
        ISmtpClientFactory smtpClientFactory,
        IOptionsSnapshot<SmtpSettings> settings,
        IRewriteReceiversService rewriteReceiversService) : ISmtpMessageSender
    {
        public async Task SendAsync(NotificationEventDTO message, CancellationToken cancellationToken)
        {
            using var client = smtpClientFactory.CreateSmtpClient();

            await client.SendMailAsync(this.ToMailMessage(message), cancellationToken);
        }

        protected virtual MailMessage ToMailMessage(NotificationEventDTO dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.Targets.All(x => x.Type != RecipientRole.To))
            {
                this.SetSupportTeamAsReceiver(dto);
            }

            var mailMessage = dto.ToDomain().ToMailMessage();
            this.TryRunRewriteRules(dto, mailMessage);

            return mailMessage;
        }

        private void TryRunRewriteRules(NotificationEventDTO dto, MailMessage mailMessage)
        {
            rewriteReceiversService.RewriteToRecipients(mailMessage, dto);
            rewriteReceiversService.RewriteCopyRecipients(mailMessage, dto);
            rewriteReceiversService.RewriteReplyTo(mailMessage, dto);
        }

        private void SetSupportTeamAsReceiver(NotificationEventDTO dto)
        {
            if (settings.Value.DefaultReceiverEmails == null)
            {
                return;
            }

            dto.Targets.AddRange(
                settings.Value.DefaultReceiverEmails.Select(x => new NotificationTargetDTO { Type = RecipientRole.To, Name = x }));
        }
    }
}

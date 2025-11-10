using System.Net.Mail;

using CommonFramework;

using Framework.Configuration.BLL;
using Framework.Notification.DTO;
using Framework.Notification.New;
using Framework.NotificationCore.Services;
using Framework.NotificationCore.Settings;

using Microsoft.Extensions.Logging;

namespace Framework.NotificationCore.Senders
{
    internal class ProdSmtpMessageSender(SmtpSettings settings, IRewriteReceiversService rewriteReceiversService, ILogger<SmtpNotificationMessageSender> logger)
        : ISmtpMessageSender
    {
        public async Task SendAsync(SmtpClient client, NotificationEventDTO message, CancellationToken cancellationToken) =>
            await client.SendMailAsync(this.ToMailMessage(message), cancellationToken);

        protected virtual MailMessage ToMailMessage(NotificationEventDTO dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            if (dto.Targets.All(x => x.Type != NotificationTargetTypes.To))
            {
                this.SetSupportTeamAsReceiver(dto);
            }

            var mailMessage = dto.ToMessage().ToMailMessage();
            this.TryRunRewriteRules(dto, mailMessage);

            return mailMessage;
        }

        private void TryRunRewriteRules(NotificationEventDTO dto, MailMessage mailMessage)
        {
            if (rewriteReceiversService == null)
            {
                return;
            }

            rewriteReceiversService.RewriteToRecipients(mailMessage, dto);
            rewriteReceiversService.RewriteCopyRecipients(mailMessage, dto);
            rewriteReceiversService.RewriteReplyTo(mailMessage, dto);
        }

        private void SetSupportTeamAsReceiver(NotificationEventDTO dto)
        {
            if (settings.DefaultReceiverEmails == null)
            {
                return;
            }

            logger.LogWarning(
                "Recipients for notification {TemplateCode} were not found - notification was redirected to support",
                dto.TechnicalInformation.MessageTemplateCode);

            dto.Targets.AddRange(
                settings.DefaultReceiverEmails.Select(
                    x => new NotificationTargetDTO { Type = NotificationTargetTypes.To, Name = x }));
        }
    }
}

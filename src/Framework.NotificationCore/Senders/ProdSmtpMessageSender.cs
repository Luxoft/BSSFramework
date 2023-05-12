using System.Net.Mail;

using Framework.Configuration.BLL;
using Framework.Core;
using Framework.Notification.DTO;
using Framework.Notification.New;
using Framework.NotificationCore.Services;
using Framework.NotificationCore.Settings;

using Serilog;

namespace Framework.NotificationCore.Senders
{
    internal class ProdSmtpMessageSender : ISmtpMessageSender
    {
        private readonly IRewriteReceiversService rewriteReceiversService;

        protected readonly SmtpSettings settings;

        public ProdSmtpMessageSender(SmtpSettings settings, IRewriteReceiversService rewriteReceiversService)
        {
            this.settings = settings;
            this.rewriteReceiversService = rewriteReceiversService;
        }

        public void Send(SmtpClient client, NotificationEventDTO message) => client.Send(this.ToMailMessage(message));

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
            if (this.rewriteReceiversService == null)
            {
                return;
            }

            this.rewriteReceiversService.RewriteToRecipients(mailMessage, dto);
            this.rewriteReceiversService.RewriteCopyRecipients(mailMessage, dto);
            this.rewriteReceiversService.RewriteReplyTo(mailMessage, dto);
        }

        private void SetSupportTeamAsReceiver(NotificationEventDTO dto)
        {
            if (this.settings.DefaultReceiverEmails == null)
            {
                return;
            }

            Log.Warning(
                $"Recipients for notification {dto.TechnicalInformation.MessageTemplateCode} were not found - notification was redirected to support");

            dto.Targets.AddRange(
                this.settings.DefaultReceiverEmails.Select(
                    x => new NotificationTargetDTO { Type = NotificationTargetTypes.To, Name = x }));
        }
    }
}

using System;
using System.Linq;
using System.Net.Mail;
using Framework.Configuration.BLL;
using Framework.Core;
using Framework.Notification.DTO;
using Framework.Notification.New;
using Framework.NotificationCore.Extensions;
using Framework.NotificationCore.Services;
using Framework.NotificationCore.Settings;
using Serilog;

namespace Framework.NotificationCore.Senders
{
    internal class ProdSmtpMessageSender : ISmtpMessageSender
    {
        protected readonly SmtpSettings settings;

        private IRewriteReceiversService rewriteReceiversService;

        public ProdSmtpMessageSender(SmtpSettings settings, IRewriteReceiversService rewriteReceiversService)
        {
            this.settings = settings;
            this.rewriteReceiversService = rewriteReceiversService;
        }

        public void Send(SmtpClient client, NotificationEventDTO message)
        {
            client.Send(this.ToMailMessage(message));
        }

        protected virtual MailMessage ToMailMessage(NotificationEventDTO dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var mailMessage = dto.ToMessage().ToMailMessage();

            if (this.rewriteReceiversService != null)
            {
                this.rewriteReceiversService.RewriteToRecipients(mailMessage, dto);
                this.rewriteReceiversService.RewriteCopyRecipients(mailMessage, dto);
                this.rewriteReceiversService.RewriteReplyTo(mailMessage, dto);
            }

            if (!mailMessage.To.Any())
            {
                Log.Warning(
                    "Recipients for notification {code} were not found - notification was redirected to support",
                    dto.TechnicalInformation.MessageTemplateCode);

                mailMessage.To.AddRange(RecipientsHelper.ToRecipients(this.settings.DefaultReceiverEmails));
            }

            return mailMessage;
        }
    }
}

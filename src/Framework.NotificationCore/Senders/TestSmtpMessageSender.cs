using System.Linq;
using System.Net.Mail;
using Framework.Core;
using Framework.Notification.DTO;
using Framework.NotificationCore.Extensions;
using Framework.NotificationCore.Services;
using Framework.NotificationCore.Settings;

namespace Framework.NotificationCore.Senders
{
    /// <summary>
    /// логика для тестовых стендов - письма перенаправляются на тестовый почтовый ящик, а исходные адресаты записываются в тело письма
    /// </summary>
    internal class TestSmtpMessageSender : ProdSmtpMessageSender
    {
        public TestSmtpMessageSender(SmtpSettings settings, IRewriteReceiversService rewriteReceiversService)
            : base(settings, rewriteReceiversService)
        {
        }

        protected override MailMessage ToMailMessage(NotificationEventDTO dto)
        {
            var message = base.ToMailMessage(dto);

            this.RedirectToTestAddress(message);

            return message;
        }

        private void RedirectToTestAddress(MailMessage message)
        {
            this.AddRecipientsToBody(message);

            message.To.Clear();
            message.CC.Clear();
            message.Bcc.Clear();
            message.ReplyToList.Clear();

            if (this.settings.TestEmails.Any())
            {
                message.To.AddRange(RecipientsHelper.ToRecipients(this.settings.TestEmails));
            }
        }

        private void AddRecipientsToBody(MailMessage message)
        {
            var originalReceivers =
                $"From: {message.From.Address}<br>" +
                $"To: {message.To.Select(x => x.Address).Join("; ")}<br>" +
                $"CC: {message.CC.Select(x => x.Address).Join("; ")}<br>" +
                $"Reply To: {message.ReplyToList.Select(x => x.Address).Join("; ")}<br><br>";

            message.Body = $"{originalReceivers}{message.Body}";
        }
    }
}

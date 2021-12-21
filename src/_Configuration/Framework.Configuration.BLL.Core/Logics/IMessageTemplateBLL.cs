using System.Collections.Generic;
using System.Net.Mail;
using Framework.Configuration.Domain;
using Framework.Notification;
using Attachment = System.Net.Mail.Attachment;

namespace Framework.Configuration.BLL
{
    public partial interface IMessageTemplateBLL
    {
        MailMessage CreateMailMessage(
            MessageTemplate messageTemplate,
            bool includeAttachments,
            object rootObject,
            MailAddress sender,
            IEnumerable<string> targetEmails,
            IEnumerable<string> carbonCopyEmails,
            IEnumerable<string> replyTo);

        MailMessage CreateMailMessage(
            bool includeAttachments,
            MessageTemplate messageTemplate,
            object rootObject,
            Dictionary<string, object> variables,
            MailAddress sender,
            IList<TargetEmail> recipients,
            IList<TargetEmail> copyRecipients,
            IList<TargetEmail> replyTo,
            IEnumerable<Attachment> attachments);

        MailMessage CreateMailMessage(
                string subject,
                string body,
                MailAddress sender,
                IList<TargetEmail> recipients,
                IList<TargetEmail> copyRecipients,
                IList<TargetEmail> replyTo,
                IEnumerable<Attachment> attachments);

        /// <summary>
        /// Создаёт сообщение электронной почты.
        /// </summary>
        /// <param name="messageTemplateNotification">Шаблон уведомления по подписке.</param>
        /// <param name="messageTemplate">Шаблон сообщения.</param>
        /// <param name="includeAttachments">if <c>true</c> прикреплять к письму вложения из шаблона сообщения.</param>
        /// <param name="rootObject">Контекстный объект, который будет использован для построения текста письма.</param>
        /// <param name="sender">Адрес электронной почты отправителя письма.</param>
        /// <param name="targetEmails">Список адресов электронной почты получателей письма.</param>
        /// <param name="carbonCopyEmails">Список адресов электронной почты CC получателей письма.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <param name="attachments">Attachments</param>
        /// <returns>Сообщение электронной почты.</returns>
        MailMessage CreateMailMessage(
            MessageTemplateNotification messageTemplateNotification,
            MessageTemplate messageTemplate,
            bool includeAttachments,
            object rootObject,
            MailAddress sender,
            IEnumerable<string> targetEmails,
            IEnumerable<string> carbonCopyEmails,
            IEnumerable<string> replyTo,
            IEnumerable<Attachment> attachments);
    }
}

using System.Collections.Immutable;
using System.Net.Mail;

using Framework.Core;

namespace Framework.Notification.Domain;

public static class MailMessageExtensions
{
    extension(MailMessage mailMessage)
    {
        public ImmutableArray<MailAddressRecipient> Recipients
        {
            get =>
            [
                .. mailMessage.To.Select(v => new MailAddressRecipient(v, RecipientRole.To)),
                .. mailMessage.CC.Select(v => new MailAddressRecipient(v, RecipientRole.Copy)),
                .. mailMessage.ReplyToList.Select(v => new MailAddressRecipient(v, RecipientRole.ReplyTo))
            ];
            set
            {
                mailMessage.To.Override(value.Where(r => r.Role == RecipientRole.To).Select(info => info.Address));
                mailMessage.CC.Override(value.Where(r => r.Role == RecipientRole.Copy).Select(info => info.Address));
                mailMessage.ReplyToList.Override(value.Where(r => r.Role == RecipientRole.ReplyTo).Select(info => info.Address));
            }
        }

        public ImmutableArray<Attachment> AttachmentList { get => [..mailMessage.Attachments]; set => mailMessage.Attachments.Override(value); }

        public MailMessage Clone() =>

            new()
            {
                Sender = mailMessage.Sender!,
                Subject = mailMessage.Subject,
                Body = mailMessage.Body,
                IsBodyHtml = mailMessage.IsBodyHtml,
                Recipients = mailMessage.Recipients,
                AttachmentList = mailMessage.AttachmentList
            };
    }
}

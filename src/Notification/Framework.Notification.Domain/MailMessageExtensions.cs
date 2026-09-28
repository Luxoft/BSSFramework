using System.Collections.Immutable;
using System.Net.Mail;

using Framework.Core;

namespace Framework.Notification.Domain;

public static class MailMessageExtensions
{
    extension(MailMessage mailMessage)
    {
        public ImmutableArray<NotificationRecipient> Recipients
        {
            get =>
            [
                .. mailMessage.To.Select(v => new NotificationRecipient(v, RecipientRole.To)),
                .. mailMessage.CC.Select(v => new NotificationRecipient(v, RecipientRole.Copy)),
                .. mailMessage.ReplyToList.Select(v => new NotificationRecipient(v, RecipientRole.ReplyTo))
            ];
            set
            {
                mailMessage.To.Override(value.Where(r => r.Role == RecipientRole.To).Select(info => info.Address));
                mailMessage.CC.Override(value.Where(r => r.Role == RecipientRole.Copy).Select(info => info.Address));
                mailMessage.ReplyToList.Override(value.Where(r => r.Role == RecipientRole.ReplyTo).Select(info => info.Address));
            }
        }

        public ImmutableArray<Attachment> AttachmentList { get => [.. mailMessage.Attachments]; set => mailMessage.Attachments.Override(value); }

        public MailMessage Clone(bool recreateAttachments = false)
        {
            var result = new MailMessage
            {
                Sender = mailMessage.Sender,
                Subject = mailMessage.Subject,
                SubjectEncoding = mailMessage.SubjectEncoding,
                Body = mailMessage.Body,
                BodyEncoding = mailMessage.BodyEncoding,
                BodyTransferEncoding = mailMessage.BodyTransferEncoding,
                IsBodyHtml = mailMessage.IsBodyHtml,
                HeadersEncoding = mailMessage.HeadersEncoding,
                Priority = mailMessage.Priority,
                DeliveryNotificationOptions = mailMessage.DeliveryNotificationOptions
            };

            if (mailMessage.From != null)
            {
                result.From = mailMessage.From;
            }

            foreach (var address in mailMessage.To)
            {
                result.To.Add(address);
            }

            foreach (var address in mailMessage.CC)
            {
                result.CC.Add(address);
            }

            foreach (var address in mailMessage.Bcc)
            {
                result.Bcc.Add(address);
            }

            foreach (var address in mailMessage.ReplyToList)
            {
                result.ReplyToList.Add(address);
            }

            foreach (var key in mailMessage.Headers.AllKeys.OfType<string>())
            {
                foreach (var value in mailMessage.Headers.GetValues(key) ?? [])
                {
                    result.Headers.Add(key, value);
                }
            }

            foreach (var attachment in mailMessage.Attachments)
            {
                result.Attachments.Add(recreateAttachments ? CloneAttachment(attachment) : attachment);
            }

            foreach (var view in mailMessage.AlternateViews)
            {
                result.AlternateViews.Add(recreateAttachments ? CloneAlternateView(view) : view);
            }

            return result;
        }
    }

    private static Attachment CloneAttachment(Attachment source)
    {
        var result = new Attachment(CopyStream(source.ContentStream), source.ContentType)
        {
            ContentId = source.ContentId,
            TransferEncoding = source.TransferEncoding,
            NameEncoding = source.NameEncoding
        };

        var sourceDisposition = source.ContentDisposition;
        var targetDisposition = result.ContentDisposition;

        if (sourceDisposition != null && targetDisposition != null)
        {
            targetDisposition.DispositionType = sourceDisposition.DispositionType;
            targetDisposition.Inline = sourceDisposition.Inline;
            targetDisposition.FileName = sourceDisposition.FileName;
            targetDisposition.CreationDate = sourceDisposition.CreationDate;
            targetDisposition.ModificationDate = sourceDisposition.ModificationDate;
            targetDisposition.ReadDate = sourceDisposition.ReadDate;

            if (sourceDisposition.Size >= 0)
            {
                targetDisposition.Size = sourceDisposition.Size;
            }
        }

        return result;
    }

    private static AlternateView CloneAlternateView(AlternateView source)
    {
        var result = new AlternateView(CopyStream(source.ContentStream), source.ContentType)
        {
            ContentId = source.ContentId,
            TransferEncoding = source.TransferEncoding,
            BaseUri = source.BaseUri
        };

        foreach (var resource in source.LinkedResources)
        {
            result.LinkedResources.Add(
                new LinkedResource(CopyStream(resource.ContentStream), resource.ContentType)
                {
                    ContentId = resource.ContentId,
                    ContentLink = resource.ContentLink,
                    TransferEncoding = resource.TransferEncoding
                });
        }

        return result;
    }

    private static MemoryStream CopyStream(Stream source)
    {
        var result = new MemoryStream();

        if (source.CanSeek)
        {
            var position = source.Position;

            source.Position = 0;
            source.CopyTo(result);
            source.Position = position;
        }
        else
        {
            source.CopyTo(result);
        }

        result.Position = 0;

        return result;
    }
}

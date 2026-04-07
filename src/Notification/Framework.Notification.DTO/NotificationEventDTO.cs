using System.Runtime.Serialization;

using Framework.Notification.Domain;

namespace Framework.Notification.DTO;

[DataContract]
public class NotificationEventDTO
{
    [DataMember]
    public List<NotificationTargetDTO> Targets { get; set; }

    [DataMember]
    public List<NotificationAttachmentDTO> Attachments { get; set; }

    [DataMember]
    public string From { get; set; }

    [DataMember]
    public string FromName { get; set; }

    [DataMember]
    public string Subject { get; set; }

    [DataMember]
    public NotificationMessage Message { get; set; }

    public NotificationEventDTO()
    {
    }

    public NotificationEventDTO(System.Net.Mail.MailMessage mailMessage)
    {
        this.From = mailMessage.From!.Address;
        this.FromName = mailMessage.From.DisplayName;

        this.Targets = mailMessage
                       .To.Select(z => Tuple.Create(z.Address, RecipientRole.To))
                       .Concat(mailMessage.CC.Select(z => Tuple.Create(z.Address, RecipientRole.Copy)))
                       .Concat(mailMessage.ReplyToList.Select(z => Tuple.Create(z.Address, RecipientRole.ReplyTo)))
                       .Select(z => new NotificationTargetDTO() { Name = z.Item1, Type = z.Item2 })
                       .ToList();
        this.Subject = mailMessage.Subject;

        this.Message = new NotificationMessage() { IsBodyHtml = mailMessage.IsBodyHtml, Message = mailMessage.Body };

        this.Attachments = mailMessage.Attachments.Select(z => new NotificationAttachmentDTO(z)).ToList();
    }
}

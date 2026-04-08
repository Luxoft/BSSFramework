using System.Runtime.Serialization;

using Framework.Notification.Domain;

namespace Framework.Notification.DTO;

[DataContract]
public class NotificationEventDTO
{
    [DataMember]
    public List<NotificationRecipientDTO> Recipients { get; set; }

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

    [DataMember]
    public NotificationTechnicalInformationDTO TechnicalInformation { get; set; }

    public NotificationEventDTO()
    {
    }

    public NotificationEventDTO(Domain.Notification notification)
    {
        var mailMessage = notification.Message;

        this.From = mailMessage.From!.Address;
        this.FromName = mailMessage.From.DisplayName;

        this.Recipients = mailMessage.Recipients.Select(mar => new NotificationRecipientDTO(mar)).ToList();
        this.Subject = mailMessage.Subject;

        this.Message = new () { IsBodyHtml = mailMessage.IsBodyHtml, Message = mailMessage.Body };

        this.Attachments = mailMessage.Attachments.Select(z => new NotificationAttachmentDTO(z)).ToList();

        this.TechnicalInformation = new (notification.TechnicalInformation);
    }
}

using System.Runtime.Serialization;

namespace Framework.Notification.DTO;

[DataContract]
public class NotificationEventDTO
{
    [DataMember]
    public IList<NotificationTargetDTO> Targets { get; set; }

    [DataMember]
    public IList<NotificationAttachmentDTO> Attachments { get; set; }

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

    public NotificationEventDTO(Notification notification)
    {
        var mailMessage = notification.Message;
        var technicalInformation = notification.TechnicalInformation;
        this.From = mailMessage.From.Address;
        this.FromName = mailMessage.From.DisplayName;

        this.Targets = mailMessage
                       .To.Select(z => Tuple.Create(z.Address, NotificationTargetTypes.To))
                       .Concat(mailMessage.CC.Select(z => Tuple.Create(z.Address, NotificationTargetTypes.Copy)))
                       .Concat(mailMessage.ReplyToList.Select(z => Tuple.Create(z.Address, NotificationTargetTypes.ReplyTo)))
                       .Select(z => new NotificationTargetDTO() { Name = z.Item1, Type = z.Item2 })
                       .ToList();
        this.Subject = mailMessage.Subject;

        this.Message = new NotificationMessage()
        {
            IsBodyHtml = mailMessage.IsBodyHtml,
            Message = mailMessage.Body
        };

        this.Attachments = mailMessage.Attachments.Select(z =>
        {
            using var ms = new MemoryStream();
            z.ContentStream.CopyTo(ms);
            byte[] content = ms.ToArray();

            return new NotificationAttachmentDTO
            {
                Content = content,
                Extension = z.ContentType.Name,
                Name = z.Name,
                ContentId = z.ContentId,
                IsInline = z.ContentDisposition.Inline
            };
        }).ToList();

        this.TechnicalInformation = new NotificationTechnicalInformationDTO(technicalInformation);
    }
}

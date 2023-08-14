using System.Runtime.Serialization;

namespace Framework.Notification.DTO;

[DataContract]
public class NotificationEventDTO
{
    [DataMember]
    public IList<NotificationTargetDTO> Targets;

    [DataMember]
    public IList<NotificationAttachmentDTO> Attachments;

    [DataMember]
    public string From;

    [DataMember]
    public string FromName;

    [DataMember]
    public string Subject;

    [DataMember]
    public NotificationMessage Message;


    [DataMember]
    public NotificationTechnicalInformationDTO TechnicalInformation;

    public NotificationEventDTO()
    {
    }
}

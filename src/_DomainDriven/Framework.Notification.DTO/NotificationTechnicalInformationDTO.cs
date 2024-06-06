using System.Runtime.Serialization;

namespace Framework.Notification.DTO;

[DataContract]
public class NotificationTechnicalInformationDTO
{
    [DataMember]
    public string MessageTemplateCode { get; set; }

    [DataMember]
    public string ContextObjectType { get; set; }

    [DataMember]
    public Guid? ContextObjectId { get; set; }

    public NotificationTechnicalInformationDTO()
    {

    }

    public NotificationTechnicalInformationDTO(NotificationTechnicalInformation information)
    {
        this.MessageTemplateCode = information.MessageTemplateCode;
        this.ContextObjectId = information.ContextObjectId;
        this.ContextObjectType = information.ContextObjectType;
    }
}

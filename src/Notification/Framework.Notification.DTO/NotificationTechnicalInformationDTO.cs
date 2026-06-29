using System.Runtime.Serialization;

using Framework.Notification.Domain;

namespace Framework.Notification.DTO;

[DataContract]
public class NotificationTechnicalInformationDTO
{
    [DataMember]
    public string MessageTemplateCode { get; set; } = null!;

    [DataMember]
    public string ContextObjectType { get; set; } = null!;

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


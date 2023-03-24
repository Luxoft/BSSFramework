using System;
using System.Runtime.Serialization;

namespace Framework.Notification.DTO;

[DataContract]
public class NotificationTechnicalInformationDTO
{
    [DataMember]
    public string MessageTemplateCode;

    [DataMember]
    public string ContextObjectType;

    [DataMember]
    public Guid? ContextObjectId;

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

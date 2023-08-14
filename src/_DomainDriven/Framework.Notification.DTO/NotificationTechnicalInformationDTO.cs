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
}

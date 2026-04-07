using System.Runtime.Serialization;

using Framework.Notification.Domain;

namespace Framework.Notification.DTO;

[DataContract]
public class NotificationRecipientDTO
{
    [DataMember]
    public RecipientRole Type { get; set; }

    [DataMember]
    public string Name { get; set; }

    public NotificationRecipientDTO()
    {
    }

    public NotificationRecipientDTO(NotificationRecipient notificationRecipient)
    {
        this.Name = notificationRecipient.Address.Address;
        this.Type = notificationRecipient.Role;
    }
}

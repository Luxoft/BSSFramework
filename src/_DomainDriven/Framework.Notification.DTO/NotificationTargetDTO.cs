using System.Runtime.Serialization;

namespace Framework.Notification.DTO;

[DataContract]
public class NotificationTargetDTO
{
    [DataMember]
    public NotificationTargetTypes Type { get; set; }

    [DataMember]
    public string Name { get; set; }
}

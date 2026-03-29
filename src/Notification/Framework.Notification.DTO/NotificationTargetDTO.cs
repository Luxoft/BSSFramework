using System.Runtime.Serialization;

using Framework.Notification.Domain;

namespace Framework.Notification.DTO;

[DataContract]
public class NotificationTargetDTO
{
    [DataMember]
    public ReceiverRole Type { get; set; }

    [DataMember]
    public string Name { get; set; }
}

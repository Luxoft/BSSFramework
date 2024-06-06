using System.Runtime.Serialization;

namespace Framework.Notification.DTO;

[DataContract]
public class NotificationMessage
{
    [DataMember]
    public bool IsBodyHtml { get; set; }

    [DataMember]
    public string Message { get; set; }
}

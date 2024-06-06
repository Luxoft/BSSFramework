using System.Runtime.Serialization;

namespace Framework.Notification.DTO;

[DataContract]
public class NotificationAttachmentDTO
{
    [DataMember]
    public byte[] Content { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string Extension { get; set; }

    [DataMember]
    public string ContentId { get; set; }

    [DataMember]
    public bool IsInline { get; set; }
}

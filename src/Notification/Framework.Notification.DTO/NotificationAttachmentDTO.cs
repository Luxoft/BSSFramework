using System.Runtime.Serialization;

using Framework.Notification.Domain;

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


    public Attachment ToDomain() => new(this.Content, this.Name) { ContentId = this.ContentId, IsInline = this.IsInline };
}

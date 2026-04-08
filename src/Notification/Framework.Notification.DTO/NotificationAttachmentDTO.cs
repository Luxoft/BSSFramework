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


    public NotificationAttachmentDTO(System.Net.Mail.Attachment attachment)
    {
        using var ms = new MemoryStream();
        attachment.ContentStream.CopyTo(ms);
        var content = ms.ToArray();

        this.Content = content;
        this.Extension = attachment.ContentType.Name;
        this.Name = attachment.Name!;
        this.ContentId = attachment.ContentId;
        this.IsInline = attachment.ContentDisposition!.Inline;
    }
}

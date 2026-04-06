using System.Runtime.Serialization;

using Framework.Notification.Domain;

namespace Framework.Notification.DTO;

[DataContract]
public class NotificationTargetDTO
{
    [DataMember]
    public RecipientRole Type { get; set; }

    [DataMember]
    public string Name { get; set; }


    public MailAddressRecipient ToDomain() => new(this.Name, this.Type);
}

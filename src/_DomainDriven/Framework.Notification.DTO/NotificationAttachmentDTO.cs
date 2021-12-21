using System.Runtime.Serialization;

namespace Framework.Notification.DTO
{
    [DataContract]
    public class NotificationAttachmentDTO
    {
        [DataMember]
        public byte[] Content;

        [DataMember]
        public string Name;

        [DataMember]
        public string Extension;

        [DataMember]
        public string ContentId;

        [DataMember]
        public bool IsInline;
    }
}

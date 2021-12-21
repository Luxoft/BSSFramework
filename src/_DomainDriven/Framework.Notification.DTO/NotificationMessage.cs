using System.Runtime.Serialization;

namespace Framework.Notification.DTO
{
    [DataContract]
    public class NotificationMessage
    {
        [DataMember]
        public bool IsBodyHtml;

        [DataMember]
        public string Message;
    }
}
using System.Runtime.Serialization;

namespace Framework.Notification.DTO
{
    [DataContract]
    public class NotificationTargetDTO
    {
        [DataMember]
        public NotificationTargetTypes Type;

        [DataMember]
        public string Name;
    }
}
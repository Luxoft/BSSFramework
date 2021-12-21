using System.Runtime.Serialization;

using Framework.DomainDriven.ServiceModel.IAD;

namespace Framework.Configuration.Generated.DTO
{
    [DataContract(Namespace = "Configuration")]
    [AutoRequest]
    public class ImportSubscriptionsRequest
    {
        [DataMember]
        [AutoRequestProperty(OrderIndex = 0)]
        public SubscriptionContainerStrictDTO Container { get; set; }

        [DataMember]
        [AutoRequestProperty(OrderIndex = 1)]
        public bool IgnoreCollision { get; set; }
    }
}

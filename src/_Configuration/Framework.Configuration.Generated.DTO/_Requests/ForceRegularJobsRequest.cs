using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.DomainDriven.ServiceModel.IAD;

namespace Framework.Configuration.Generated.DTO
{
    [DataContract(Namespace = "Configuration")]
    [AutoRequest]
    public class ForceRegularJobsRequest
    {
        [DataMember]
        [AutoRequestProperty(OrderIndex = 0)]
        public List<RegularJobIdentityDTO> RegularJobs { get; set; }

        [DataMember]
        [AutoRequestProperty(OrderIndex = 1)]
        public RunRegularJobMode Mode { get; set; }
    }
}

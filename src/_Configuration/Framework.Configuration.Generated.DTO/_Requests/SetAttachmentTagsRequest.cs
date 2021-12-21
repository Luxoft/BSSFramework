using System.Collections.Generic;
using System.Runtime.Serialization;

using Framework.DomainDriven.ServiceModel.IAD;

namespace Framework.Configuration.Generated.DTO
{
    [DataContract(Namespace = "Configuration")]
    [AutoRequest]
    public class SetAttachmentTagsRequest
    {
        [DataMember]
        [AutoRequestProperty(OrderIndex = 0)]
        public AttachmentIdentityDTO Attachment { get; set; }

        [DataMember]
        [AutoRequestProperty(OrderIndex = 0)]
        public List<AttachmentTagStrictDTO> Tags { get; set; }
    }
}

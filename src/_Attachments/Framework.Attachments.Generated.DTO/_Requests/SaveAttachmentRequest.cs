using System;
using System.Runtime.Serialization;

using Framework.DomainDriven.ServiceModel.IAD;

namespace Framework.Attachments.Generated.DTO
{
    [DataContract(Namespace = "Configuration")]
    [AutoRequest]
    public class SaveAttachmentRequest
    {
        [DataMember]
        [AutoRequestProperty(OrderIndex = 0)]
        public AttachmentContainerReferenceStrictDTO Reference { get; set; }

        [DataMember]
        [AutoRequestProperty(OrderIndex = 1)]
        public AttachmentStrictDTO Attachment { get; set; }
    }
}

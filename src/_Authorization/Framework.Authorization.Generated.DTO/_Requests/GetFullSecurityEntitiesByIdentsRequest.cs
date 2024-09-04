using System.Runtime.Serialization;

using Framework.DomainDriven.ServiceModel.IAD;

namespace Framework.Authorization.Generated.DTO;

[DataContract(Namespace = "Auth")]
[AutoRequest]
public class GetFullSecurityEntitiesByIdentsRequest
{
    [DataMember]
    [AutoRequestProperty(OrderIndex = 0)]
    public SecurityContextTypeIdentityDTO SecurityContextType { get; set; }

    [DataMember]
    [AutoRequestProperty(OrderIndex = 1)]
    public List<SecurityEntityIdentityDTO> SecurityEntities { get; set; }
}

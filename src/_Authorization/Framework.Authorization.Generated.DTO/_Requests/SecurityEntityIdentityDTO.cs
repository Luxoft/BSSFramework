using System.Runtime.Serialization;

using Framework.Application.Domain;

namespace Framework.Authorization.Generated.DTO;

[DataContract(Namespace = "Auth")]
public struct SecurityEntityIdentityDTO(Guid id) : IIdentityObject<Guid>
{
    [DataMember]
    public Guid Id { get; set; } = id;
}

using System;
using System.Runtime.Serialization;

using Framework.Persistent;

namespace Framework.Authorization.Generated.DTO
{
    [DataContractAttribute(Namespace = "Auth")]
    public partial struct SecurityEntityIdentityDTO : IIdentityObject<Guid>
    {
        public SecurityEntityIdentityDTO(Guid id)
            : this()
        {
            this.Id = id;
        }


        [DataMember]
        public Guid Id { get; set; }
    }
}
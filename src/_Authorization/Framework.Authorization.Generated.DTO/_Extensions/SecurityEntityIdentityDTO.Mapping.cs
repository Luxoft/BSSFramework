using System;
using System.Runtime.Serialization;
using Framework.Authorization.Domain;
using Framework.Persistent;

namespace Framework.Authorization.Generated.DTO;

public partial struct SecurityEntityIdentityDTO
{
    public SecurityEntity ToDomainObject()
    {
        return new SecurityEntity { Id = this.Id };
    }
}

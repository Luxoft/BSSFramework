using Framework.Authorization.Domain;

namespace Framework.Authorization.Generated.DTO;

public partial struct SecurityEntityIdentityDTO
{
    public SecurityEntity ToDomainObject()
    {
        return new SecurityEntity { Id = this.Id };
    }
}

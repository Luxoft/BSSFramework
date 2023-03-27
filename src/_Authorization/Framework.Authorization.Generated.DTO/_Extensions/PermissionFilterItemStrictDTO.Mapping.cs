using Framework.Authorization.Domain;

namespace Framework.Authorization.Generated.DTO;

public partial class AuthorizationServerPrimitiveDTOMappingService
{
    public override void MapPermissionFilterItem(PermissionFilterItemStrictDTO mappingObject, PermissionFilterItem domainObject)
    {
        base.MapPermissionFilterItem(mappingObject, domainObject);

        if (domainObject.Entity == null)
        {
            domainObject.Entity = this.GetEntityFunc(mappingObject.EntityType, mappingObject.SecurityEntity);
        }
    }
}

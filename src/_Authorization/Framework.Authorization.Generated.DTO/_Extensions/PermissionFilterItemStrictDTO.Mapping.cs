using Framework.Authorization.Domain;

namespace Framework.Authorization.Generated.DTO;

public partial class AuthorizationServerPrimitiveDTOMappingService
{
    public override void MapPermissionRestriction(PermissionRestrictionStrictDTO mappingObject, PermissionRestriction domainObject)
    {
        base.MapPermissionRestriction(mappingObject, domainObject);

        if (domainObject.Entity == null)
        {
            domainObject.Entity = this.GetEntityFunc(mappingObject.EntityType, mappingObject.SecurityEntity);
        }
    }
}

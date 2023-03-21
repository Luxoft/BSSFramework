using Framework.Authorization.Domain;

namespace Framework.Authorization.Generated.DTO;

public partial class DelegateToItemModelStrictDTO
{
    internal Permission DelegatedFromPermission;
}

public partial class AuthorizationServerPrimitiveDTOMappingService
{
    public override void MapChangePermissionDelegatesModel(ChangePermissionDelegatesModelStrictDTO mappingObject, ChangePermissionDelegatesModel domainObject)
    {
        var delegatedFromPermission = mappingObject.DelegateFromPermission.ToDomainObject(this);

        foreach (var item in mappingObject.Items)
        {
            item.DelegatedFromPermission = delegatedFromPermission;
        }

        base.MapChangePermissionDelegatesModel(mappingObject, domainObject);
    }

    public override void MapUpdatePermissionDelegatesModel(UpdatePermissionDelegatesModelStrictDTO mappingObject, UpdatePermissionDelegatesModel domainObject)
    {
        var delegatedFromPermission = mappingObject.DelegateFromPermission.ToDomainObject(this);

        foreach (var item in mappingObject.AddItems)
        {
            item.DelegatedFromPermission = delegatedFromPermission;
        }

        base.MapUpdatePermissionDelegatesModel(mappingObject, domainObject);
    }

    public override void MapDelegateToItemModel(DelegateToItemModelStrictDTO mappingObject, DelegateToItemModel domainObject)
    {
        base.MapDelegateToItemModel(mappingObject, domainObject);

        if (domainObject.Permission == null && domainObject.Principal != null)
        {
            domainObject.Permission = new Permission(domainObject.Principal, mappingObject.DelegatedFromPermission);
        }

        if (domainObject.Permission != null)
        {
            mappingObject.Permission.MapToDomainObject(this, domainObject.Permission);
        }
    }
}

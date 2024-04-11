using Framework.Authorization.Domain;

namespace Framework.Authorization.BLL;

public partial interface IPermissionRestrictionBLL
{
    void NotifySave(PermissionRestriction permissionRestriction);

    void NotifyRemove(PermissionRestriction permissionRestriction);
}

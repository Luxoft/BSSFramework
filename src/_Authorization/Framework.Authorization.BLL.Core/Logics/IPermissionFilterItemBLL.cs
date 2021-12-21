using Framework.Authorization.Domain;

namespace Framework.Authorization.BLL
{
    public partial interface IPermissionFilterItemBLL
    {
        void NotifySave(PermissionFilterItem permissionFilterItem);

        void NotifyRemove(PermissionFilterItem permissionFilterItem);
    }
}
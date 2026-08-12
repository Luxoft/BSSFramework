using Framework.Authorization.Domain;

namespace Framework.Authorization.BLL;

public partial interface IPermissionBLL
{
    void ChangeDelegatePermissions(ChangePermissionDelegatesModel changePermissionDelegatesModel);

    void UpdateDelegatePermissions(UpdatePermissionDelegatesModel updatePermissionDelegatesModel);
}

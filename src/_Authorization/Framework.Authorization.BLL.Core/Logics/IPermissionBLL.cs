using Framework.Authorization.Domain;

namespace Framework.Authorization.BLL;

public partial interface IPermissionBLL
{
    void Save(Permission permission, bool withValidate);

    void ChangeDelegatePermissions(ChangePermissionDelegatesModel changePermissionDelegatesModel);

    void UpdateDelegatePermissions(UpdatePermissionDelegatesModel updatePermissionDelegatesModel);

    void WithdrawDelegation(Permission permission);
}

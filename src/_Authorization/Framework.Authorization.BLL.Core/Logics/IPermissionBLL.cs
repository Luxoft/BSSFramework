using Framework.Authorization.Domain;

namespace Framework.Authorization.BLL;

public partial interface IPermissionBLL
{
    void Save(Permission permission, bool withValidate);

    void DenormalizePermission(Permission permission);

    void ValidatePermissionDelegated(Permission permission, ValidatePermissonDelegateMode mode);

    void ChangeDelegatePermissions(ChangePermissionDelegatesModel changePermissionDelegatesModel);

    void UpdateDelegatePermissions(UpdatePermissionDelegatesModel updatePermissionDelegatesModel);

    void WithdrawDelegation(Permission permission);
}

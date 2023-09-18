using Framework.Authorization.Domain;

namespace Framework.Authorization.BLL;

public partial interface IPermissionBLL
{
    void Save(Permission permission, bool withValidate);

    void DenormalizePermission(Permission permission);

    void ValidatePermissionDelegated(Permission permission, ValidatePermissonDelegateMode mode);

    void ValidateApprovingPermission(Permission permission);

    void ChangeDelegatePermissions(ChangePermissionDelegatesModel changePermissionDelegatesModel);

    void UpdateDelegatePermissions(UpdatePermissionDelegatesModel updatePermissionDelegatesModel);

    void WithdrawDelegation(Permission permission);
}


[Flags]
public enum ValidatePermissonDelegateMode
{
    Role = 1,

    Period = 2,

    SecurityObjects = 4,

    ApproveState = 8,

    All = Role + Period + SecurityObjects + ApproveState
}

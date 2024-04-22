using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystem;

public interface IPermissionValidator
{
    void Validate(Permission permission);

    void ValidateDelegation(Permission permission, ValidatePermissionDelegateMode mode = ValidatePermissionDelegateMode.All);
}

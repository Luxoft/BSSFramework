using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystem;

public interface IPermissionValidator
{
    void Validate(Permission permission);
}

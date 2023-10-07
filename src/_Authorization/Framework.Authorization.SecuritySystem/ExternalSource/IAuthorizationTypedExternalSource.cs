using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystem.ExternalSource;

public interface IAuthorizationTypedExternalSource : IAuthorizationTypedExternalSourceBase
{
    PermissionFilterEntity AddSecurityEntity(SecurityEntity securityEntity, bool disableExistsCheck = false);
}

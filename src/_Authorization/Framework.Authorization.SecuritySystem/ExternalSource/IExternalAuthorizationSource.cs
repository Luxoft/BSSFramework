using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystem.ExternalSource;

public interface IAuthorizationExternalSource
{
    IAuthorizationTypedExternalSource GetTyped(SecurityContextType securityContextType, bool withCache = true);
}

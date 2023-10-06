using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystem.ExternalSource;

public interface IAuthorizationExternalSource
{
    IAuthorizationTypedExternalSource GetTyped(EntityType entityType, bool withCache = true);
}

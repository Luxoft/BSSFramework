using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystemImpl;

public interface ICurrentPrincipalSource
{
    Principal CurrentPrincipal { get; }
}

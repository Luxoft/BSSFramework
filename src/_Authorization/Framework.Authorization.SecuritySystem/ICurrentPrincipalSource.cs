using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystem;

public interface ICurrentPrincipalSource
{
    Principal CurrentPrincipal { get; }
}

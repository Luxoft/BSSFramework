using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystem;

public interface IActualPrincipalSource
{
    Principal ActualPrincipal { get; }
}

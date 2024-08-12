using Framework.Authorization.Domain;
using Framework.Core;

namespace Framework.Authorization.SecuritySystem;

public class ActualPrincipalSource(ICurrentPrincipalSource currentPrincipalSource) : IActualPrincipalSource
{
    public Principal ActualPrincipal => currentPrincipalSource.CurrentPrincipal.Pipe(v => v.RunAs ?? v);
}

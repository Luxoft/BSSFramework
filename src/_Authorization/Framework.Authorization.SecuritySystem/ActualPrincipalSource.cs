using Framework.Authorization.Domain;
using Framework.Core;

namespace Framework.Authorization.SecuritySystem;

public class ActualPrincipalSource : IActualPrincipalSource
{
    private readonly ICurrentPrincipalSource currentPrincipalSource;

    public ActualPrincipalSource(ICurrentPrincipalSource currentPrincipalSource)
    {
        this.currentPrincipalSource = currentPrincipalSource;
    }

    public Principal ActualPrincipal => this.currentPrincipalSource.CurrentPrincipal.Pipe(v => v.RunAs ?? v);
}

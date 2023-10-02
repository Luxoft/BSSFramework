using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystem;

public class ActualPrincipalSource : IActualPrincipalSource
{
    private readonly Principal currentPrincipal;

    public ActualPrincipalSource(ICurrentPrincipalSource currentPrincipalSource)
    {
        this.currentPrincipal = currentPrincipalSource.CurrentPrincipal;
    }

    public Principal ActualPrincipal => this.currentPrincipal.RunAs ?? this.currentPrincipal;
}

using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystem;

public interface IRunAsManager
{
    Principal ActualPrincipal { get; }

    bool IsRunningAs { get; }

    Task StartRunAsUserAsync(string principalName, CancellationToken cancellationToken = default);

    Task FinishRunAsUserAsync(CancellationToken cancellationToken = default);
}

namespace Framework.Authorization.SecuritySystem;

public interface IRunAsManager
{
    bool IsRunningAs { get; }

    Task StartRunAsUserAsync(string principalName, CancellationToken cancellationToken = default);

    Task FinishRunAsUserAsync(CancellationToken cancellationToken = default);
}

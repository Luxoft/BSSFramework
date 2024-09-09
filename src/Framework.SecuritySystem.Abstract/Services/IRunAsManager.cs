namespace Framework.SecuritySystem.Services;

public interface IRunAsManager
{
    string? RunAsName { get; }

    Task StartRunAsUserAsync(string principalName, CancellationToken cancellationToken = default);

    Task FinishRunAsUserAsync(CancellationToken cancellationToken = default);
}

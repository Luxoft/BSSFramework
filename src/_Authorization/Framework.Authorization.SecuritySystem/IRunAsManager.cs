namespace Framework.Authorization.SecuritySystem;

public interface IRunAsManager
{
    string PrincipalName { get; }

    bool IsRunningAs { get; }

    Task StartRunAsUser(string principalName, CancellationToken cancellationToken = default);

    Task FinishRunAsUser(CancellationToken cancellationToken = default);
}

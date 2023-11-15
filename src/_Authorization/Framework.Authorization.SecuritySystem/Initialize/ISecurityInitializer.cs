namespace Framework.Authorization.SecuritySystem.Initialize;

public interface ISecurityInitializer
{
    Task Init(CancellationToken cancellationToken = default);
}

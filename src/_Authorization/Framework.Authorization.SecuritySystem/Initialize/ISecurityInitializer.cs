namespace Framework.Authorization.SecuritySystemImpl.Initialize;

public interface ISecurityInitializer
{
    Task Init(CancellationToken cancellationToken = default);
}

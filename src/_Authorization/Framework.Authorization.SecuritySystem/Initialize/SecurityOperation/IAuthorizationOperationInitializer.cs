namespace Framework.Authorization.SecuritySystem.Initialize;

public interface IAuthorizationOperationInitializer : ISecurityInitializer
{
    Task RemoveUnusedAsync(CancellationToken cancellationToken = default);
}

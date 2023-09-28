namespace Framework.Authorization.SecuritySystem.OperationInitializer;

public interface IAuthorizationOperationInitializer
{
    public Task InitSecurityOperations(UnexpectedAuthOperationMode mode, CancellationToken cancellationToken = default);
}

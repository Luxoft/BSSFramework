namespace Framework.Authorization.SecuritySystem.OperationInitializer;

public interface IAuthorizationOperationInitializer
{
    Task InitSecurityOperations(UnexpectedAuthOperationMode mode, CancellationToken cancellationToken = default);
}

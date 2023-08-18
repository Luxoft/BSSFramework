namespace Framework.SecuritySystem;

public static class AuthorizationSystemExtensions
{
    public static void CheckAccess<TSecurityOperationCode>(
            this IAuthorizationSystem authorizationSystem,
            IAccessDeniedExceptionService accessDeniedExceptionService,
            NonContextSecurityOperation<TSecurityOperationCode> operation)
            where TSecurityOperationCode : struct, Enum
    {
        if (!authorizationSystem.HasAccess(operation))
        {
            throw accessDeniedExceptionService.GetAccessDeniedException($"You are not authorized to perform {operation} operation");
        }
    }
}

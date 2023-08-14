using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public static class AuthorizationBLLContextExtensions
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

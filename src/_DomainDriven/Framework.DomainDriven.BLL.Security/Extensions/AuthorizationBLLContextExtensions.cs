using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public static class AuthorizationBLLContextExtensions
{
    public static void CheckAccess<TBllContext, TSecurityOperationCode>(
            this TBllContext context,
            NonContextSecurityOperation<TSecurityOperationCode> operation,
            bool withRunAs = true)
            where TBllContext : IAuthorizationBLLContextBase, IAccessDeniedExceptionServiceContainer
            where TSecurityOperationCode : struct, Enum
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        if (!context.HasAccess(operation, withRunAs))
        {
            throw context.AccessDeniedExceptionService.GetAccessDeniedException($"You are not authorized to perform {operation} operation");
        }
    }
}

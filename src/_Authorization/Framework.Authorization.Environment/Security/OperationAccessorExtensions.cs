using Framework.SecuritySystem;

namespace Framework.Authorization.Environment.Security;

public static class OperationAccessorExtensions
{
    public static bool IsSecurityAdministrator(this IOperationAccessor operationAccessor) =>
        operationAccessor.HasAccess(AuthorizationSecurityRule.SecurityAdministrator);
}

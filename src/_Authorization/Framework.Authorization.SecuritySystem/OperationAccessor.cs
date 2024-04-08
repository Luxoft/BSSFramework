using Framework.Authorization.Domain;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class OperationAccessor : IOperationAccessor
{
    private readonly IAvailablePermissionSource availablePermissionSource;

    private readonly IAccessDeniedExceptionService accessDeniedExceptionService;

    private readonly bool withRunAs;

    public OperationAccessor(
        IAvailablePermissionSource availablePermissionSource,
        IAccessDeniedExceptionService accessDeniedExceptionService,
        bool withRunAs)
    {
        this.availablePermissionSource = availablePermissionSource;
        this.accessDeniedExceptionService = accessDeniedExceptionService;
        this.withRunAs = withRunAs;
    }

    public bool IsAdmin() => this.availablePermissionSource.GetAvailablePermissionsQueryable(this.withRunAs)
                                 .Any(permission => permission.Role.Name == BusinessRole.AdminRoleName);

    public bool HasAccess(SecurityRule securityRule)
    {
        var typedOperation = (SecurityRule)securityRule;

        return this.availablePermissionSource.GetAvailablePermissionsQueryable(securityOperationId: typedOperation.Id, withRunAs: this.withRunAs).Any();
    }

    public void CheckAccess(SecurityRule securityRule)
    {
        if (!this.HasAccess(securityRule))
        {
            throw this.accessDeniedExceptionService.GetAccessDeniedException(
                new AccessResult.AccessDeniedResult { SecurityRule = securityRule });
        }
    }
}

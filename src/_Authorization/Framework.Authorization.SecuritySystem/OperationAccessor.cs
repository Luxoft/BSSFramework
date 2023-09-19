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

    public bool HasAccess(NonContextSecurityOperation securityOperation)
    {
        var typedOperation = (NonContextSecurityOperation<Guid>)securityOperation;

        return this.availablePermissionSource.GetAvailablePermissionsQueryable(securityOperationId: typedOperation.Id, withRunAs: this.withRunAs).Any();
    }

    public void CheckAccess(NonContextSecurityOperation securityOperation)
    {
        if (!this.HasAccess(securityOperation))
        {
            throw this.accessDeniedExceptionService.GetAccessDeniedException(
                new AccessResult.AccessDeniedResult { SecurityOperation = securityOperation });
        }
    }
}

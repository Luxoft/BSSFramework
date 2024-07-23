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

    public bool HasAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return this.availablePermissionSource.GetAvailablePermissionsQueryable(securityRule: securityRule, withRunAs: this.withRunAs).Any();
    }

    public void CheckAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        if (!this.HasAccess(securityRule))
        {
            throw this.accessDeniedExceptionService.GetAccessDeniedException(
                new AccessResult.AccessDeniedResult { SecurityRule = securityRule });
        }
    }
}

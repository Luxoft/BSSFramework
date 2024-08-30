namespace Framework.SecuritySystem;

public abstract class SecuritySystemBase(IAccessDeniedExceptionService accessDeniedExceptionService) : ISecuritySystem
{
    public abstract bool HasAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule);

    public void CheckAccess(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        if (!this.HasAccess(securityRule))
        {
            throw accessDeniedExceptionService.GetAccessDeniedException(
                new AccessResult.AccessDeniedResult { SecurityRule = securityRule });
        }
    }
}

using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class SecurityRolesIdentsResolver(
    ISecurityRuleExpander securityRuleExpander,
    ISecurityRoleSource securityRoleSource)
    : ISecurityRolesIdentsResolver
{
    public IEnumerable<Guid> Resolve(SecurityRule.DomainObjectSecurityRule securityRule)
    {
        return securityRuleExpander.FullExpand(securityRule)
                                   .SelectMany(rule => rule.SecurityRoles)
                                   .Distinct()
                                   .Select(sr => securityRoleSource.GetFullRole(sr).Id);
    }
}

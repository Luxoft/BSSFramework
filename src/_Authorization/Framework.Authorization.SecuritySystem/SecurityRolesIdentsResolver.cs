using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public class SecurityRolesIdentsResolver(
    ISecurityRuleExpander securityRuleExpander,
    ISecurityRoleSource securityRoleSource)
    : ISecurityRolesIdentsResolver
{
    public IEnumerable<Guid> Resolve(SecurityRule.ExpandableSecurityRule securityRule)
    {
        return securityRuleExpander.FullExpand(securityRule)
                                   .SelectMany(rule => rule.SecurityRoles)
                                   .Distinct()
                                   .Select(securityRoleSource.GetFullRole)
                                   .Where(sr => !sr.IsVirtual)
                                   .Select(sr => sr.Id);
    }
}

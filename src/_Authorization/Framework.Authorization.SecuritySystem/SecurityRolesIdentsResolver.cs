using Framework.SecuritySystem;
using Framework.SecuritySystem.Expanders;

namespace Framework.Authorization.SecuritySystem;

public class SecurityRolesIdentsResolver(
    ISecurityRuleExpander securityRuleExpander,
    ISecurityRoleSource securityRoleSource)
    : ISecurityRolesIdentsResolver
{
    public IEnumerable<Guid> Resolve(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return securityRuleExpander.FullExpand(securityRule)
                                   .SelectMany(rule => rule.SecurityRoles)
                                   .Distinct()
                                   .Select(securityRoleSource.GetSecurityRole)
                                   .Where(sr => !sr.IsVirtual)
                                   .Select(sr => sr.Id);
    }
}

using Framework.SecuritySystem.Expanders;

namespace Framework.SecuritySystem;

public class SecurityRolesIdentsResolver(
    ISecurityRuleExpander securityRuleExpander,
    ISecurityRoleSource securityRoleSource)
    : ISecurityRolesIdentsResolver
{
    public IEnumerable<Guid> Resolve(DomainSecurityRule.RoleBaseSecurityRule securityRule, bool includeVirtual = false)
    {
        return securityRuleExpander.FullRoleExpand(securityRule)
                                   .SecurityRoles
                                   .Select(securityRoleSource.GetSecurityRole)
                                   .Where(sr => includeVirtual || !sr.Information.IsVirtual)
                                   .Select(sr => sr.Id);
    }
}

using Framework.SecuritySystem.Expanders;

namespace Framework.SecuritySystem;

public class SecurityRolesIdentsResolver(
    ISecurityRuleExpander securityRuleExpander,
    ISecurityRoleSource securityRoleSource)
    : ISecurityRolesIdentsResolver
{
    public IEnumerable<Guid> Resolve(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        return securityRuleExpander.FullExpand(securityRule)
                                   .SecurityRoles
                                   .Select(securityRoleSource.GetSecurityRole)
                                   .Where(sr => !sr.Information.IsVirtual)
                                   .Select(sr => sr.Id);
    }
}

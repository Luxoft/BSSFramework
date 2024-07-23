using Framework.Core;

namespace Framework.SecuritySystem.Expanders;

public class SecurityRoleExpander
{
    private readonly IDictionaryCache<DomainSecurityRule.NonExpandedRolesSecurityRule, DomainSecurityRule.ExpandedRolesSecurityRule> expandCache;

    public SecurityRoleExpander(ISecurityRoleSource securityRoleSource)
    {
        this.expandCache = new DictionaryCache<DomainSecurityRule.NonExpandedRolesSecurityRule, DomainSecurityRule.ExpandedRolesSecurityRule>(
            securityRule =>
            {
                if (securityRule.SecurityRoles.Count == 0)
                {
                    throw new Exception("The list of security roles cannot be empty.");
                }

                var securityRoles = securityRoleSource.SecurityRoles
                                                      .Where(
                                                          sr => sr.GetAllElements(
                                                                      c => c.Information.Children.Select(securityRoleSource.GetSecurityRole))
                                                                  .IsIntersected(securityRule.SecurityRoles))
                                                      .Concat(securityRule.SecurityRoles)
                                                      .Distinct()
                                                      .OrderBy(sr => sr.Name)
                                                      .ToArray();

                return new DomainSecurityRule.ExpandedRolesSecurityRule(DeepEqualsCollection.Create(securityRoles))
                       {
                           CustomExpandType = securityRule.CustomExpandType
                       };
            }).WithLock();
    }

    public DomainSecurityRule.ExpandedRolesSecurityRule Expand(DomainSecurityRule.NonExpandedRolesSecurityRule securityRule)
    {
        return this.expandCache[securityRule];
    }
}

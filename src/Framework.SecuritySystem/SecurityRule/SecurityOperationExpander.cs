#nullable enable

using Framework.Core;

namespace Framework.SecuritySystem;

public class SecurityOperationExpander : ISecurityRuleExpander
{
    private readonly IDictionaryCache<SecurityRule, SecurityRule?> tryExpandCache;

    public SecurityOperationExpander(ISecurityRoleSource securityRoleSource)
    {
        this.tryExpandCache = new DictionaryCache<SecurityRule, SecurityRule?>(
            securityRule =>
            {
                if (securityRule is SecurityRule.OperationSecurityRule operationRule)
                {
                    var securityRoles = securityRoleSource.SecurityRoles.GetAllElements(sr => sr.Children)
                                                          .Where(sr => sr.Operations.Contains(operationRule.SecurityOperation))
                                                          .Distinct()
                                                          .ToArray();

                    return securityRoles.ToArray();
                }
                else
                {
                    return null;
                }
            });
    }

    public SecurityRule.RolesSecurityRule Expand<TDomainObject>(SecurityRule.DomainObjectSecurityRule securityRule)
    {
        if (securityRule is SecurityRule.RolesSecurityRule rolesSecurityRule)
        {
            return rolesSecurityRule;
        }
        else
        {

        }
        return this.tryExpandCache[securityRule];
    }

    public SecurityRule? TryExpand<TDomainObject>(SecurityRule securityRule)
    {
        return this.tryExpandCache[securityRule];
    }
}

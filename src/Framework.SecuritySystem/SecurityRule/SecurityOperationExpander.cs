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
                if (securityRule is SecurityRule<SecurityOperation> operationRule)
                {
                    var securityOperation = operationRule.Value;

                    var securityRoles = securityRoleSource.SecurityRoles.GetAllElements(sr => sr.Children)
                                                          .Where(sr => sr.Operations.Contains(securityOperation))
                                                          .Distinct()
                                                          .ToArray();

                    return new SecurityRule<SecurityRole[]>(securityRoles);
                }
                else
                {
                    return null;
                }
            });
    }

    public SecurityRule? TryExpand<TDomainObject>(SecurityRule securityRule)
    {
        return this.tryExpandCache[securityRule];
    }
}

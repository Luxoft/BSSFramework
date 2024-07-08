using Framework.Core;
using Framework.SecuritySystem.Providers.Operation;
using Framework.SecuritySystem.Rules.Builders;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem;

public class SecurityPathProviderFactory(
    IServiceProvider serviceProvider,
    ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
    ISecurityRuleExpander securityRuleExpander,
    ISecurityRoleSource securityRoleSource,
    ISecurityPathRestrictionService securityPathRestrictionService) : ISecurityPathProviderFactory
{
    public ISecurityProvider<TDomainObject> Create<TDomainObject>(SecurityPath<TDomainObject> securityPath, SecurityRule.DomainObjectSecurityRule rootSecurityRule)
    {
        switch (rootSecurityRule)
        {
            case SecurityRule.ExpandableSecurityRule expandedRolesSecurityRule:
                return this.GetRegroupedRoles(expandedRolesSecurityRule)
                           .Select(g => this.Create(securityPath, g.SecurityRule, g.Restriction)).Or();

            case SecurityRule.CustomProviderSecurityRule customProviderSecurityRule:
            {
                var securityProviderFactoryType =
                    customProviderSecurityRule.SecurityProviderFactoryType.MakeGenericType(typeof(TDomainObject));

                return (ISecurityProvider<TDomainObject>)serviceProvider.GetRequiredService(securityProviderFactoryType);
            }

            case SecurityRule.OrSecurityRule orSecurityRule:
                return this.Create(securityPath, orSecurityRule.Left).Or(this.Create(securityPath, orSecurityRule.Right));

            case SecurityRule.AndSecurityRule andSecurityRule:
                return this.Create(securityPath, andSecurityRule.Left).And(this.Create(securityPath, andSecurityRule.Right));

            default:
                throw new ArgumentOutOfRangeException(nameof(rootSecurityRule));

        }
    }

    private ISecurityProvider<TDomainObject> Create<TDomainObject>(SecurityPath<TDomainObject> securityPath, SecurityRule.ExpandableSecurityRule securityRule, SecurityPathRestriction restriction)
    {
        return new ContextSecurityPathProvider<TDomainObject>(
            securityPathRestrictionService.ApplyRestriction(securityPath, restriction),
            securityRule,
            securityExpressionBuilderFactory);
    }

    private IEnumerable<(SecurityRule.ExpandedRolesSecurityRule SecurityRule, SecurityPathRestriction Restriction)> GetRegroupedRoles(SecurityRule.ExpandableSecurityRule rootSecurityRule)
    {
        return from expandedSecurityRule in securityRuleExpander.FullExpand(rootSecurityRule)

               from securityRole in expandedSecurityRule.SecurityRoles

               let securityRoleInfo = securityRoleSource.GetFullRole(securityRole).Information

               let actualCustomExpandType = rootSecurityRule.CustomExpandType ?? expandedSecurityRule.CustomExpandType ?? securityRoleInfo.CustomExpandType

               group securityRole by new { actualCustomExpandType, securityRoleInfo.Restriction } into g

               let rule = new SecurityRule.ExpandedRolesSecurityRule(DeepEqualsCollection.Create(g)) { CustomExpandType = g.Key.actualCustomExpandType }

               select (rule, g.Key.Restriction);
    }
}

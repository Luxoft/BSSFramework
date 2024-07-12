using System.Linq.Expressions;

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
    public virtual ISecurityProvider<TDomainObject> Create<TDomainObject>(SecurityPath<TDomainObject> securityPath, SecurityRule.DomainObjectSecurityRule securityRule)
    {
        switch (securityRule)
        {
            case SecurityRule.ExpandableSecurityRule expandedRolesSecurityRule:
                return this.GetRegroupedRoles(expandedRolesSecurityRule)
                           .Select(g => this.Create(securityPath, g.SecurityRule, g.Restriction)).Or();

            case SecurityRule.CustomProviderSecurityRule customProviderSecurityRule:
            {
                var securityProviderType =
                    customProviderSecurityRule.GenericSecurityProviderType.MakeGenericType(typeof(TDomainObject));

                var securityProvider = customProviderSecurityRule.Key == null
                                           ? serviceProvider.GetRequiredService(securityProviderType)
                                           : serviceProvider.GetRequiredKeyedService(securityProviderType, customProviderSecurityRule.Key);

                return (ISecurityProvider<TDomainObject>)securityProvider;
            }

            case SecurityRule.CustomProviderFactorySecurityRule customProviderFactorySecurityRule:
            {
                var securityProviderFactoryType =
                    customProviderFactorySecurityRule.GenericSecurityProviderFactoryType.MakeGenericType(typeof(TDomainObject));

                var securityProviderFactory = customProviderFactorySecurityRule.Key == null
                                           ? serviceProvider.GetRequiredService(securityProviderFactoryType)
                                           : serviceProvider.GetRequiredKeyedService(securityProviderFactoryType, customProviderFactorySecurityRule.Key);

                return ((IFactory<ISecurityProvider<TDomainObject>>)securityProviderFactory).Create();
            }

            case SecurityRule.ConditionSecurityRule conditionSecurityRule:
            {
                var conditionFactoryType =
                    conditionSecurityRule.GenericConditionFactoryType.MakeGenericType(typeof(TDomainObject));

                var conditionFactory = (IFactory<Expression<Func<TDomainObject, bool>>>)serviceProvider.GetRequiredService(conditionFactoryType);

                var condition = conditionFactory.Create();

                return SecurityProvider<TDomainObject>.Create(condition);
            }

            case SecurityRule.OrSecurityRule orSecurityRule:
                return this.Create(securityPath, orSecurityRule.Left).Or(this.Create(securityPath, orSecurityRule.Right));

            case SecurityRule.AndSecurityRule andSecurityRule:
                return this.Create(securityPath, andSecurityRule.Left).And(this.Create(securityPath, andSecurityRule.Right));

            case SecurityRule.NegateSecurityRule negateSecurityRule:
                return this.Create(securityPath, negateSecurityRule.InnerRule).Negate();

            default:
                throw new ArgumentOutOfRangeException(nameof(securityRule));

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

               let securityRoleInfo = securityRoleSource.GetSecurityRole(securityRole).Information

               let actualCustomExpandType = rootSecurityRule.CustomExpandType ?? expandedSecurityRule.CustomExpandType ?? securityRoleInfo.CustomExpandType

               group securityRole by new { actualCustomExpandType, securityRoleInfo.Restriction } into g

               let rule = new SecurityRule.ExpandedRolesSecurityRule(DeepEqualsCollection.Create(g)) { CustomExpandType = g.Key.actualCustomExpandType }

               select (rule, g.Key.Restriction);
    }
}

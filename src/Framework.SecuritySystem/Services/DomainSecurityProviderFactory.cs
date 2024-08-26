using System.Linq.Expressions;
using Framework.Core;
using Framework.SecuritySystem.UserSource;

using Microsoft.Extensions.DependencyInjection;

using static Framework.SecuritySystem.DomainSecurityRule;

namespace Framework.SecuritySystem.Services;

public class DomainSecurityProviderFactory(
    IServiceProvider serviceProvider,
    ISecurityRuleDeepOptimizer deepOptimizer,
    ISecurityRuleImplementationResolver implementationResolver,
    IRoleBaseSecurityProviderFactory roleBaseSecurityProviderFactory) : IDomainSecurityProviderFactory
{
    public virtual ISecurityProvider<TDomainObject> Create<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        DomainSecurityRule securityRule)
    {
        return this.CreateInternal(securityPath, deepOptimizer.Optimize(securityRule));
    }

    protected virtual ISecurityProvider<TDomainObject> CreateInternal<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        DomainSecurityRule baseSecurityRule)
    {
        switch (baseSecurityRule)
        {
            case RoleBaseSecurityRule securityRule:
                return roleBaseSecurityProviderFactory.Create(securityPath, securityRule);

            case CurrentUserSecurityRule securityRule:
            {
                var args = securityRule.RelativePathKey == null
                              ? []
                              : new object[] { new CurrentUserSecurityProviderRelativeKey(securityRule.RelativePathKey) };

                return ActivatorUtilities.CreateInstance<CurrentUserSecurityProvider<TDomainObject>>(serviceProvider, args);
            }

            case ProviderSecurityRule securityRule:
            {
                var securityProviderType =
                    securityRule.GenericSecurityProviderType.MakeGenericType(typeof(TDomainObject));

                var securityProvider = securityRule.Key == null
                                           ? serviceProvider.GetRequiredService(securityProviderType)
                                           : serviceProvider.GetRequiredKeyedService(securityProviderType, securityRule.Key);

                return (ISecurityProvider<TDomainObject>)securityProvider;
            }

            case ProviderFactorySecurityRule securityRule:
            {
                var securityProviderFactoryType =
                    securityRule.GenericSecurityProviderFactoryType.MakeGenericType(typeof(TDomainObject));

                var securityProviderFactoryUntyped =
                    securityRule.Key == null
                        ? serviceProvider.GetRequiredService(securityProviderFactoryType)
                        : serviceProvider.GetRequiredKeyedService(securityProviderFactoryType, securityRule.Key);

                var securityProviderFactory = (IFactory<ISecurityProvider<TDomainObject>>)securityProviderFactoryUntyped;

                return securityProviderFactory.Create();
            }

            case ConditionFactorySecurityRule securityRule:
            {
                var conditionFactoryType =
                    securityRule.GenericConditionFactoryType.MakeGenericType(typeof(TDomainObject));

                var conditionFactoryUntyped = serviceProvider.GetRequiredService(conditionFactoryType);

                var conditionFactory = (IFactory<Expression<Func<TDomainObject, bool>>>)conditionFactoryUntyped;

                return SecurityProvider<TDomainObject>.Create(conditionFactory.Create());
            }

            case RelativeConditionSecurityRule securityRule:
            {
                var conditionInfo = securityRule.RelativeConditionInfo;

                var factoryType = typeof(RequiredRelativeConditionFactory<,>).MakeGenericType(
                    typeof(TDomainObject),
                    conditionInfo.RelativeDomainObjectType);

                var untypedConditionFactory = ActivatorUtilities.CreateInstance(serviceProvider, factoryType, conditionInfo);

                var conditionFactory = (IFactory<Expression<Func<TDomainObject, bool>>>)untypedConditionFactory;

                var condition = conditionFactory.Create();

                return SecurityProvider<TDomainObject>.Create(condition);
            }

            case FactorySecurityRule securityRule:
            {
                var dynamicRoleFactoryUntyped = serviceProvider.GetRequiredService(securityRule.RuleFactoryType);

                var dynamicRoleFactory = (IFactory<DomainSecurityRule>)dynamicRoleFactoryUntyped;

                return this.CreateInternal(securityPath, dynamicRoleFactory.Create());
            }

            case OverrideAccessDeniedMessageSecurityRule securityRule:
            {
                return this.CreateInternal(securityPath, securityRule.BaseSecurityRule)
                           .OverrideAccessDeniedResult(
                               accessDeniedResult => accessDeniedResult with { CustomMessage = securityRule.CustomMessage });
            }

            case SecurityRuleHeader securityRuleHeader:
                return this.CreateInternal(securityPath, implementationResolver.Resolve(securityRuleHeader));

            case OrSecurityRule securityRule:
                return this.CreateInternal(securityPath, securityRule.Left).Or(this.CreateInternal(securityPath, securityRule.Right));

            case AndSecurityRule securityRule:
                return this.CreateInternal(securityPath, securityRule.Left).And(this.CreateInternal(securityPath, securityRule.Right));

            case NegateSecurityRule securityRule:
                return this.CreateInternal(securityPath, securityRule.InnerRule).Negate();

            default:
                throw new ArgumentOutOfRangeException(nameof(baseSecurityRule));
        }
    }
}

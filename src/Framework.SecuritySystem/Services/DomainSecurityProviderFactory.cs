using System.Linq.Expressions;

using Microsoft.Extensions.DependencyInjection;

using Framework.Core;
using Framework.SecuritySystem.UserSource;

using static Framework.SecuritySystem.DomainSecurityRule;

namespace Framework.SecuritySystem.Services;

public class DomainSecurityProviderFactory<TDomainObject>(
    IServiceProvider serviceProvider,
    ISecurityRuleDeepOptimizer deepOptimizer,
    ISecurityRuleImplementationResolver implementationResolver,
    IRoleBaseSecurityProviderFactory<TDomainObject> roleBaseSecurityProviderFactory) : IDomainSecurityProviderFactory<TDomainObject>
{
    public virtual ISecurityProvider<TDomainObject> Create(
        DomainSecurityRule securityRule,
        SecurityPath<TDomainObject> securityPath)
    {
        return this.CreateInternal(deepOptimizer.Optimize(securityRule), securityPath);
    }

    protected virtual ISecurityProvider<TDomainObject> CreateInternal(
        DomainSecurityRule baseSecurityRule,
        SecurityPath<TDomainObject> securityPath)
    {
        switch (baseSecurityRule)
        {
            case RoleBaseSecurityRule securityRule:
                return roleBaseSecurityProviderFactory.Create(securityRule, securityPath);

            case CurrentUserSecurityRule securityRule:
            {
                var args = new object?[]
                    {
                        securityRule.RelativePathKey == null
                            ? null
                            : new CurrentUserSecurityProviderRelativeKey(securityRule.RelativePathKey)
                    }.Where(arg => arg != null)
                     .ToArray(arg => arg!);

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

                return this.CreateInternal(dynamicRoleFactory.Create(), securityPath);
            }

            case OverrideAccessDeniedMessageSecurityRule securityRule:
            {
                return this.CreateInternal(securityRule.BaseSecurityRule, securityPath)
                           .OverrideAccessDeniedResult(
                               accessDeniedResult => accessDeniedResult with { CustomMessage = securityRule.CustomMessage });
            }

            case SecurityRuleHeader securityRuleHeader:
                return this.CreateInternal(implementationResolver.Resolve(securityRuleHeader), securityPath);

            case OrSecurityRule securityRule:
                return this.CreateInternal(securityRule.Left, securityPath).Or(this.CreateInternal(securityRule.Right, securityPath));

            case AndSecurityRule securityRule:
                return this.CreateInternal(securityRule.Left, securityPath).And(this.CreateInternal(securityRule.Right, securityPath));

            case NegateSecurityRule securityRule:
                return this.CreateInternal(securityRule.InnerRule, securityPath).Negate();

            default:
                throw new ArgumentOutOfRangeException(nameof(baseSecurityRule));
        }
    }
}

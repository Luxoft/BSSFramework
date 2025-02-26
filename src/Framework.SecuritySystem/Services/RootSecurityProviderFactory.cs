using System.Linq.Expressions;

using Microsoft.Extensions.DependencyInjection;

using Framework.Core;
using Framework.SecuritySystem.UserSource;

using static Framework.SecuritySystem.DomainSecurityRule;
using Framework.SecuritySystem.ProviderFactories;
using Framework.SecuritySystem.Expanders;

namespace Framework.SecuritySystem.Services;

public class RootSecurityProviderFactory<TDomainObject, TSecurityRule>(
    ISecurityRuleDeepOptimizer deepOptimizer,
    IDefaultSecurityProviderFactory<TDomainObject, TSecurityRule> defaultSecurityProviderFactory,
    IEnumerable<ISecurityProviderInjector<TDomainObject, TSecurityRule>> injectors) : ISecurityProviderFactory<TDomainObject, TSecurityRule>
    where TSecurityRule : DomainSecurityRule
{
    public ISecurityProvider<TDomainObject> Create(TSecurityRule securityRule, SecurityPath<TDomainObject>? securityPath)
    {
        var optimizeSettings = defaultSecurityProviderFactory.AllowOptimize
                                   ? new SecurityRuleExpandSettings(injectors.Select(injector => injector.SecurityRuleType))
                                   : SecurityRuleExpandSettings.Disabled;

        var actualSecurityRule = deepOptimizer.Optimize(securityRule, optimizeSettings);

        injectors.Aggregate(defaultFactory, (state, injector) => injector.Inject(state)).Create(securityRule, securityPath);
    }
}

public class RootSecurityProviderFactory<TDomainObject>(
    IServiceProvider serviceProvider,
    ISecurityRuleTypeResolver securityRuleTypeResolver,
    ISecurityRuleDeepOptimizer deepOptimizer) : IRootSecurityProviderFactory<TDomainObject>
{
    public ISecurityProvider<TDomainObject> Create(SecurityRule securityRule, SecurityPath<TDomainObject>? securityPath)
    {
        var securityRuleType = securityRuleTypeResolver.Resolve(securityRule);

        var injectorType = typeof(ISecurityProviderInjector<,>).MakeGenericType(typeof(TDomainObject), securityRuleType);

        var defaultFactory = serviceProvider.GetRequiredService(
            typeof(IDefaultSecurityProviderFactory<,>).MakeGenericType(injectorType, securityRuleType));

        var injectors = serviceProvider.GetRequiredService(typeof(IEnumerable<>).MakeGenericType(injectorType));


        switch (baseSecurityRule)
        {
            case DomainSecurityRule securityRule:
                return this.CreateInternal(deepOptimizer.Optimize(securityRule), securityPath);

            case SecurityRule.ModeSecurityRule securityRule:
                return this.Create(securityRule.ToDomain<TDomainObject>(), securityPath);

            default:
                throw new ArgumentOutOfRangeException(nameof(baseSecurityRule));
        }
    }

    private class InternalDomainSecurityProviderFactory<TSecurityRule>(
        DomainSecurityProviderFactory<TDomainObject> rootFactory,
        ISecurityProviderFactory<TDomainObject, TSecurityRule> defaultFactory,
        IEnumerable<ISecurityProviderInjector<TDomainObject, TSecurityRule>> injectors,
        ISecurityRuleDeepOptimizer deepOptimizer,
        TSecurityRule securityRule,
        SecurityPath<TDomainObject> securityPath) : IFactory<ISecurityProvider<TDomainObject>>
        where TSecurityRule : SecurityRule
    {
        public ISecurityProvider<TDomainObject> Create()
        {
            if (!injectors.Any())
            {

            }
            else
            {
                injectors.Aggregate(defaultFactory, (state, injector) => injector.Inject(state)).Create(securityRule, securityPath);
            }
        }
    }

    private ISecurityProvider<TDomainObject> Create<TSecurityRule>(
        TSecurityRule securityRule,
        SecurityPath<TDomainObject> securityPath,
        ISecurityProviderFactory<TDomainObject, TSecurityRule> defaultFactory,
        IEnumerable<ISecurityProviderInjector<TDomainObject, TSecurityRule>> injectors)
        where TSecurityRule : SecurityRule =>
        injectors.Aggregate(defaultFactory, (state, injector) => injector.Inject(state)).Create(securityRule, securityPath);
}

internal class InternalDomainSecurityProviderFactory<TDomainObject, TSecurityRule>(
    IServiceProvider serviceProvider,
    IRoleBaseSecurityProviderFactory<TDomainObject> roleBaseSecurityProviderFactory,
    SecurityPath<TDomainObject> securityPath,
    ISecurityProviderInjector<TDomainObject, TSecurityRule> injectors)
    where TSecurityRule : SecurityRule
{
    protected virtual ISecurityProvider<TDomainObject> Create(DomainSecurityRule baseSecurityRule)
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

                    return this.Create(dynamicRoleFactory.Create());
                }

            case OverrideAccessDeniedMessageSecurityRule securityRule:
                {
                    return this.Create(securityRule.BaseSecurityRule)
                               .OverrideAccessDeniedResult(
                                   accessDeniedResult => accessDeniedResult with { CustomMessage = securityRule.CustomMessage });
                }

            case OrSecurityRule securityRule:
                return this.Create(securityRule.Left).Or(this.Create(securityRule.Right));

            case AndSecurityRule securityRule:
                return this.Create(securityRule.Left).And(this.Create(securityRule.Right));

            case NegateSecurityRule securityRule:
                return this.Create(securityRule.InnerRule).Negate();

            case DomainModeSecurityRule:
            case SecurityRuleHeader:
            case ClientSecurityRule:
                throw new Exception("Must be optimized");

            default:
                throw new ArgumentOutOfRangeException(nameof(baseSecurityRule));
        }
    }
}

using System.Linq.Expressions;

using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem;

public class DomainSecurityProviderFactory(
    IServiceProvider serviceProvider,
    IRoleBaseSecurityProviderFactory roleBaseSecurityProviderFactory) : IDomainSecurityProviderFactory
{
    public virtual ISecurityProvider<TDomainObject> Create<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        SecurityRule.DomainSecurityRule securityRule)
    {
        switch (securityRule)
        {
            case SecurityRule.RoleBaseSecurityRule expandedRolesSecurityRule:
                return roleBaseSecurityProviderFactory.Create(securityPath, expandedRolesSecurityRule);

            case SecurityRule.ProviderSecurityRule providerSecurityRule:
            {
                var securityProviderType =
                    providerSecurityRule.GenericSecurityProviderType.MakeGenericType(typeof(TDomainObject));

                var securityProvider = providerSecurityRule.Key == null
                                           ? serviceProvider.GetRequiredService(securityProviderType)
                                           : serviceProvider.GetRequiredKeyedService(securityProviderType, providerSecurityRule.Key);

                return (ISecurityProvider<TDomainObject>)securityProvider;
            }

            case SecurityRule.ProviderFactorySecurityRule providerFactorySecurityRule:
            {
                var securityProviderFactoryType =
                    providerFactorySecurityRule.GenericSecurityProviderFactoryType.MakeGenericType(typeof(TDomainObject));

                var securityProviderFactoryUntyped =
                    providerFactorySecurityRule.Key == null
                        ? serviceProvider.GetRequiredService(securityProviderFactoryType)
                        : serviceProvider.GetRequiredKeyedService(securityProviderFactoryType, providerFactorySecurityRule.Key);

                var securityProviderFactory = (IFactory<ISecurityProvider<TDomainObject>>)securityProviderFactoryUntyped;

                return securityProviderFactory.Create();
            }

            case SecurityRule.ConditionSecurityRule conditionSecurityRule:
            {
                var conditionFactoryType =
                    conditionSecurityRule.GenericConditionFactoryType.MakeGenericType(typeof(TDomainObject));

                var conditionFactoryUntyped = serviceProvider.GetRequiredService(conditionFactoryType);

                var conditionFactory = (IFactory<Expression<Func<TDomainObject, bool>>>)conditionFactoryUntyped;

                return SecurityProvider<TDomainObject>.Create(conditionFactory.Create());
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
}

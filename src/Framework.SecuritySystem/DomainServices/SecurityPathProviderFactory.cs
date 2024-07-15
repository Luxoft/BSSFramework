using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem;

public class SecurityPathProviderFactory(
    IServiceProvider serviceProvider,
    IRoleBaseSecurityProviderFactory roleBaseSecurityProviderFactory) : ISecurityPathProviderFactory
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

                var securityProviderFactory = providerFactorySecurityRule.Key == null
                                                  ? serviceProvider.GetRequiredService(securityProviderFactoryType)
                                                  : serviceProvider.GetRequiredKeyedService(
                                                      securityProviderFactoryType,
                                                      providerFactorySecurityRule.Key);

                return ((IFactory<ISecurityProvider<TDomainObject>>)securityProviderFactory).Create();
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

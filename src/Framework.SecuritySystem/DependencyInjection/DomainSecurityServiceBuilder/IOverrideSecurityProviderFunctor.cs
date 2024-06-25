namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public interface IOverrideSecurityProviderFunctor<TDomainObject>
{
    ISecurityProvider<TDomainObject> OverrideSecurityProvider(ISecurityProvider<TDomainObject> baseProvider, SecurityRule.SpecialSecurityRule securityRule) => baseProvider;

    ISecurityProvider<TDomainObject> OverrideSecurityProvider(ISecurityProvider<TDomainObject> baseProvider, SecurityRule.OperationSecurityRule securityRule) => baseProvider;

    ISecurityProvider<TDomainObject> OverrideSecurityProvider(ISecurityProvider<TDomainObject> baseProvider, SecurityRule.NonExpandedRolesSecurityRule securityRule) => baseProvider;

    ISecurityProvider<TDomainObject> OverrideSecurityProvider(ISecurityProvider<TDomainObject> baseProvider, SecurityRule.ExpandedRolesSecurityRule securityRule) => baseProvider;

    ISecurityProvider<TDomainObject> OverrideSecurityProvider(ISecurityProvider<TDomainObject> baseProvider, SecurityRule.CompositeSecurityRule securityRule) => baseProvider;
}

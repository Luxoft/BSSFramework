namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public interface IOverrideSecurityProviderFunctor<TDomainObject>
{
    ISecurityProvider<TDomainObject> OverrideSecurityProvider(ISecurityProvider<TDomainObject> baseProvider, SecurityRule securityRule) => baseProvider;
}

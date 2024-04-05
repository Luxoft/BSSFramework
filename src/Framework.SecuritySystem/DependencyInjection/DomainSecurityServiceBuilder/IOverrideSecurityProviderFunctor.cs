namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public interface IOverrideSecurityProviderFunctor<TDomainObject>
{
    ISecurityProvider<TDomainObject> OverrideSecurityProvider(ISecurityProvider<TDomainObject> baseProvider, SecurityRule securityMode)
    {
        return baseProvider;
    }

    ISecurityProvider<TDomainObject> OverrideSecurityProvider(ISecurityProvider<TDomainObject> baseProvider, SecurityOperation securityRule)
    {
        return baseProvider;
    }
}

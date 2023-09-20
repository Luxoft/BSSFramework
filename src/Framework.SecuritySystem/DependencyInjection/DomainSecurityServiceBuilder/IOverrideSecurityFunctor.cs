namespace Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

public interface IOverrideSecurityFunctor<TDomainObject>
{
    ISecurityProvider<TDomainObject> Override(ISecurityProvider<TDomainObject> baseProvider, SecurityOperation securityOperation);
}

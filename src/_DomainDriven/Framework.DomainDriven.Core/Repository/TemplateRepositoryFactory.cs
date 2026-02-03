using CommonFramework;

using SecuritySystem;

using SecuritySystem.DomainServices;
using SecuritySystem.Providers;

namespace Framework.DomainDriven.Repository;

public abstract class TemplateRepositoryFactory<TRepository, TRepositoryImpl, TDomainObject>(
    IServiceProxyFactory serviceProxyFactory,
    IDomainSecurityService<TDomainObject> domainSecurityService)
    : ITemplateGenericRepositoryFactory<TRepository, TDomainObject>
    where TDomainObject : class
    where TRepositoryImpl : TRepository
{
    public TRepository Create() => this.Create(SecurityRule.Disabled);

    public TRepository Create(SecurityRule securityRule) =>
        this.Create(domainSecurityService.GetSecurityProvider(securityRule));

    public TRepository Create(ISecurityProvider<TDomainObject> securityProvider) =>
        serviceProxyFactory.Create<TRepository, TRepositoryImpl>(securityProvider);
}

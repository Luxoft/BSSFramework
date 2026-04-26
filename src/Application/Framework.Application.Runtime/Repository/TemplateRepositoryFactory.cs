using Anch.Core;

using Anch.SecuritySystem;
using Anch.SecuritySystem.DomainServices;
using Anch.SecuritySystem.Providers;

namespace Framework.Application.Repository;

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

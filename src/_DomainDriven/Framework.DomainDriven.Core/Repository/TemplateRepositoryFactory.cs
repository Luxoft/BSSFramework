using SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.DomainServices;
using SecuritySystem.Providers;

namespace Framework.DomainDriven.Repository;

public abstract class TemplateRepositoryFactory<TRepository, TTRepositoryImpl, TDomainObject>(
    IServiceProvider serviceProvider,
    IDomainSecurityService<TDomainObject> domainSecurityService)
    : ITemplateGenericRepositoryFactory<TRepository, TDomainObject>
    where TDomainObject : class
    where TTRepositoryImpl : TRepository
{
    public TRepository Create() => this.Create(SecurityRule.Disabled);

    public TRepository Create(SecurityRule securityRule) =>
        this.Create(domainSecurityService.GetSecurityProvider(securityRule));

    public TRepository Create(ISecurityProvider<TDomainObject> securityProvider) =>
        ActivatorUtilities.CreateInstance<TTRepositoryImpl>(serviceProvider, securityProvider);
}

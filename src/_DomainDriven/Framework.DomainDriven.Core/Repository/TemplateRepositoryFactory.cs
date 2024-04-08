using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Repository;

public abstract class TemplateRepositoryFactory<TRepository, TTRepositoryImpl, TDomainObject> :
    ITemplateGenericRepositoryFactory<TRepository, TDomainObject>
    where TDomainObject : class
    where TTRepositoryImpl : TRepository
{
    private readonly IServiceProvider serviceProvider;

    private readonly IDomainSecurityService<TDomainObject> domainSecurityService;

    protected TemplateRepositoryFactory(
        IServiceProvider serviceProvider,
        IDomainSecurityService<TDomainObject> domainSecurityService)
    {
        this.serviceProvider = serviceProvider;
        this.domainSecurityService = domainSecurityService;
    }

    public TRepository Create() =>
        this.Create(this.domainSecurityService.GetSecurityProvider(SecurityRule.Disabled));

    public TRepository Create(SecurityRule securityMode) =>
        this.Create(this.domainSecurityService.GetSecurityProvider(securityMode));

    public TRepository Create(SecurityRule securityRule) =>
        this.Create(this.domainSecurityService.GetSecurityProvider(securityRule));

    public TRepository Create(ISecurityProvider<TDomainObject> securityProvider) =>
        ActivatorUtilities.CreateInstance<TTRepositoryImpl>(this.serviceProvider, securityProvider);
}

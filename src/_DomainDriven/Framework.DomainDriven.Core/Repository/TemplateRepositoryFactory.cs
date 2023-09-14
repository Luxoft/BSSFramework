using Framework.DomainDriven.Repository.NotImplementedDomainSecurityService;
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
        INotImplementedDomainSecurityService<TDomainObject> notImplementedDomainSecurityService,
        IDomainSecurityService<TDomainObject>? domainSecurityService = null)
    {
        this.serviceProvider = serviceProvider;
        this.domainSecurityService = domainSecurityService ?? notImplementedDomainSecurityService;
    }

    public TRepository Create() =>
        this.Create(this.domainSecurityService.GetSecurityProvider(BLLSecurityMode.Disabled));

    public TRepository Create(BLLSecurityMode securityMode) =>
        this.Create(this.domainSecurityService.GetSecurityProvider(securityMode));

    public TRepository Create(SecurityOperation securityOperation) =>
        this.Create(this.domainSecurityService.GetSecurityProvider(securityOperation));

    public TRepository Create(ISecurityProvider<TDomainObject> securityProvider) =>
        ActivatorUtilities.CreateInstance<TTRepositoryImpl>(this.serviceProvider, securityProvider);
}

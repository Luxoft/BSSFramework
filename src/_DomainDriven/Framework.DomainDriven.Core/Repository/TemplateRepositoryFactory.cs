using Framework.DomainDriven.Repository.NotImplementedDomainSecurityService;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Repository;

public abstract class TemplateRepositoryFactory<TRepository, TTRepositoryImpl, TDomainObject> : TemplateRepositoryFactory<TRepository, TTRepositoryImpl, TDomainObject>,
    ITemplateGenericRepositoryFactory<TRepository, TDomainObject>
        where TDomainObject : class
        where TSecurityOperationCode : struct, Enum
        where TTRepositoryImpl : TRepository
{
    private readonly IDomainSecurityService<TDomainObject> domainSecurityService;

    protected TemplateRepositoryFactory(
            IServiceProvider serviceProvider,
            INotImplementedDomainSecurityService<TDomainObject> notImplementedDomainSecurityService,
            IDomainSecurityService<TDomainObject>? domainSecurityService = null)
        :base(serviceProvider, notImplementedDomainSecurityService, domainSecurityService)
    {
        this.domainSecurityService = domainSecurityService ?? notImplementedDomainSecurityService;
    }

    public TRepository Create(SecurityOperation securityOperation) =>
            this.Create(this.domainSecurityService.GetSecurityProvider(securityOperationCode));

    public TRepository Create(SecurityOperation securityOperation) =>
            this.Create(this.domainSecurityService.GetSecurityProvider(securityOperation));
}

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

    public TRepository Create(ISecurityProvider<TDomainObject> securityProvider) =>
        ActivatorUtilities.CreateInstance<TTRepositoryImpl>(this.serviceProvider, securityProvider);

    public TRepository Create(BLLSecurityMode securityMode) =>
        this.Create(this.domainSecurityService.GetSecurityProvider(securityMode));

    public TRepository Create() =>
        this.Create(this.domainSecurityService.GetSecurityProvider(BLLSecurityMode.Disabled));
}

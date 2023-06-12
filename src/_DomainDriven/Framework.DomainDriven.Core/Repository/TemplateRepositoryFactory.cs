using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Repository;

public abstract class TemplateRepositoryFactory<TRepository, TTRepositoryImpl, TDomainObject, TSecurityOperationCode> : TemplateRepositoryFactory<TRepository, TTRepositoryImpl, TDomainObject>,
    ITemplateGenericRepositoryFactory<TRepository, TDomainObject, TSecurityOperationCode>
        where TDomainObject : class
        where TSecurityOperationCode : struct, Enum
        where TTRepositoryImpl : TRepository
{
    private readonly IDomainSecurityService<TDomainObject, TSecurityOperationCode> domainSecurityService;

    protected TemplateRepositoryFactory(
            IServiceProvider serviceProvider,
            INotImplementedDomainSecurityServiceContainer notImplementedDomainSecurityServiceContainer,
            IDomainSecurityService<TDomainObject, TSecurityOperationCode>? domainSecurityService = null)
        :base(serviceProvider, notImplementedDomainSecurityServiceContainer, domainSecurityService)
    {
        this.domainSecurityService = domainSecurityService ?? notImplementedDomainSecurityServiceContainer.GetNotImplementedDomainSecurityService<TDomainObject, TSecurityOperationCode>();
    }

    public TRepository Create(TSecurityOperationCode securityOperationCode) =>
            this.Create(this.domainSecurityService.GetSecurityProvider(securityOperationCode));

    public TRepository Create(SecurityOperation<TSecurityOperationCode> securityOperation) =>
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
        INotImplementedDomainSecurityServiceContainer notImplementedDomainSecurityServiceContainer,
        IDomainSecurityService<TDomainObject>? domainSecurityService = null)
    {
        this.serviceProvider = serviceProvider;
        this.domainSecurityService = domainSecurityService ?? notImplementedDomainSecurityServiceContainer.GetNotImplementedDomainSecurityService<TDomainObject>();
    }

    public TRepository Create(ISecurityProvider<TDomainObject> securityProvider) =>
        ActivatorUtilities.CreateInstance<TTRepositoryImpl>(this.serviceProvider, securityProvider);

    public TRepository Create(BLLSecurityMode securityMode) =>
        this.Create(this.domainSecurityService.GetSecurityProvider(securityMode));

    public TRepository Create() =>
        this.Create(this.domainSecurityService.GetSecurityProvider(BLLSecurityMode.Disabled));
}

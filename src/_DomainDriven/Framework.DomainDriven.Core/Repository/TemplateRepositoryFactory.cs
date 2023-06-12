using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Repository;

public class TemplateRepositoryFactory<TRepository, TTRepositoryImpl, TDomainObject, TIdent, TSecurityOperationCode> :
    ITemplateGenericRepositoryFactory<TRepository, TDomainObject, TIdent, TSecurityOperationCode>
        where TDomainObject : class
        where TSecurityOperationCode : struct, Enum
        where TRepository : IGenericRepository<TDomainObject, TIdent>
        where TTRepositoryImpl : TRepository
{
    private readonly IServiceProvider serviceProvider;

    private readonly IDomainSecurityService<TDomainObject, TSecurityOperationCode> domainSecurityService;

    public TemplateRepositoryFactory(
            IServiceProvider serviceProvider,
            INotImplementedDomainSecurityServiceContainer notImplementedDomainSecurityServiceContainer,
            IDomainSecurityService<TDomainObject, TSecurityOperationCode>? domainSecurityService = null)
    {
        this.serviceProvider = serviceProvider;
        this.domainSecurityService = domainSecurityService ?? notImplementedDomainSecurityServiceContainer.GetNotImplementedDomainSecurityService<TDomainObject, TSecurityOperationCode>();
    }

    public TRepository Create(ISecurityProvider<TDomainObject> securityProvider) =>
            ActivatorUtilities.CreateInstance<TTRepositoryImpl>(this.serviceProvider, securityProvider);

    public TRepository Create(TSecurityOperationCode securityOperationCode) =>
            this.Create(this.domainSecurityService.GetSecurityProvider(securityOperationCode));

    public TRepository Create(SecurityOperation<TSecurityOperationCode> securityOperation) =>
            this.Create(this.domainSecurityService.GetSecurityProvider(securityOperation));

    public TRepository Create(BLLSecurityMode securityMode) =>
            this.Create(this.domainSecurityService.GetSecurityProvider(securityMode));

    public TRepository Create() =>
            this.Create(this.domainSecurityService.GetSecurityProvider(BLLSecurityMode.Disabled));
}

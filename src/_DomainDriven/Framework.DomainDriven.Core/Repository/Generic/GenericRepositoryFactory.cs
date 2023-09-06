using Framework.DomainDriven.Repository.NotImplementedDomainSecurityService;
using Framework.SecuritySystem;



namespace Framework.DomainDriven.Repository;

public class GenericRepositoryFactory<TDomainObject, TIdent, TSecurityOperationCode> : TemplateRepositoryFactory<
    IGenericRepository<TDomainObject, TIdent>,
    GenericRepository<TDomainObject, TIdent>,
    TDomainObject,
    TSecurityOperationCode>,

    IGenericRepositoryFactory<TDomainObject, TIdent, TSecurityOperationCode>

    where TDomainObject : class
    where TSecurityOperationCode : struct, Enum
{
    public GenericRepositoryFactory(
        IServiceProvider serviceProvider,
        INotImplementedDomainSecurityService<TDomainObject, TSecurityOperationCode> notImplementedDomainSecurityService,
        IDomainSecurityService<TDomainObject, TSecurityOperationCode> domainSecurityService = null)
        : base(serviceProvider, notImplementedDomainSecurityService, domainSecurityService)
    {
    }
}

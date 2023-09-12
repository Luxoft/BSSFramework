using Framework.DomainDriven.Repository.NotImplementedDomainSecurityService;
using Framework.SecuritySystem;



namespace Framework.DomainDriven.Repository;

public class GenericRepositoryFactory<TDomainObject, TIdent> : TemplateRepositoryFactory<
    IGenericRepository<TDomainObject, TIdent>,
    GenericRepository<TDomainObject, TIdent>,
    TDomainObject,
    TSecurityOperationCode>,

    IGenericRepositoryFactory<TDomainObject, TIdent>

    where TDomainObject : class
    where TSecurityOperationCode : struct, Enum
{
    public GenericRepositoryFactory(
        IServiceProvider serviceProvider,
        INotImplementedDomainSecurityService<TDomainObject> notImplementedDomainSecurityService,
        IDomainSecurityService<TDomainObject> domainSecurityService = null)
        : base(serviceProvider, notImplementedDomainSecurityService, domainSecurityService)
    {
    }
}

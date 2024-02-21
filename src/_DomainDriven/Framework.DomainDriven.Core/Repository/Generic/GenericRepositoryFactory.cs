using Framework.SecuritySystem;

namespace Framework.DomainDriven.Repository;

public class GenericRepositoryFactory<TDomainObject, TIdent> : TemplateRepositoryFactory<
    IGenericRepository<TDomainObject, TIdent>,
    GenericRepository<TDomainObject, TIdent>,
    TDomainObject>,

    IGenericRepositoryFactory<TDomainObject, TIdent>

    where TDomainObject : class
{
    public GenericRepositoryFactory(
        IServiceProvider serviceProvider,
        IDomainSecurityService<TDomainObject> domainSecurityService )
        : base(serviceProvider, domainSecurityService)
    {
    }
}

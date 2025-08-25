using SecuritySystem.DomainServices;

namespace Framework.DomainDriven.Repository;

public class GenericRepositoryFactory<TDomainObject, TIdent>(
    IServiceProvider serviceProvider,
    IDomainSecurityService<TDomainObject> domainSecurityService)
    : TemplateRepositoryFactory<
      IGenericRepository<TDomainObject, TIdent>,
      GenericRepository<TDomainObject, TIdent>,
      TDomainObject>(serviceProvider, domainSecurityService),
      IGenericRepositoryFactory<TDomainObject, TIdent>
    where TDomainObject : class;

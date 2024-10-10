using Framework.SecuritySystem;

namespace Framework.DomainDriven.Repository;

public class RepositoryFactory<TDomainObject>(
    IServiceProvider serviceProvider,
    IDomainSecurityService<TDomainObject> domainSecurityService)
    : TemplateRepositoryFactory<
      IRepository<TDomainObject>,
      Repository<TDomainObject>,
      TDomainObject>(serviceProvider, domainSecurityService),
      IRepositoryFactory<TDomainObject>
    where TDomainObject : class;

using CommonFramework;

using SecuritySystem.DomainServices;

namespace Framework.DomainDriven.Repository;

public class RepositoryFactory<TDomainObject>(IServiceProxyFactory serviceProxyFactory, IDomainSecurityService<TDomainObject> domainSecurityService)
    : TemplateRepositoryFactory<
      IRepository<TDomainObject>,
      Repository<TDomainObject>,
      TDomainObject>(serviceProxyFactory, domainSecurityService),
      IRepositoryFactory<TDomainObject>
    where TDomainObject : class;

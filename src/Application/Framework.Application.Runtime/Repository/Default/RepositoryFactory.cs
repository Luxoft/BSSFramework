using Anch.Core;

using Anch.SecuritySystem.DomainServices;

namespace Framework.Application.Repository.Default;

public class RepositoryFactory<TDomainObject>(IServiceProxyFactory serviceProxyFactory, IDomainSecurityService<TDomainObject> domainSecurityService)
    : TemplateRepositoryFactory<
      IRepository<TDomainObject>,
      Repository<TDomainObject>,
      TDomainObject>(serviceProxyFactory, domainSecurityService),
      IRepositoryFactory<TDomainObject>
    where TDomainObject : class;

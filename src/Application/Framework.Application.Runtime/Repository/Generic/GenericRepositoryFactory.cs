using Anch.Core;

using Anch.SecuritySystem.DomainServices;

namespace Framework.Application.Repository.Generic;

public class GenericRepositoryFactory<TDomainObject, TIdent>(IServiceProxyFactory serviceProxyFactory, IDomainSecurityService<TDomainObject> domainSecurityService)
    : TemplateRepositoryFactory<
      IGenericRepository<TDomainObject, TIdent>,
      GenericRepository<TDomainObject, TIdent>,
      TDomainObject>(serviceProxyFactory, domainSecurityService),
      IGenericRepositoryFactory<TDomainObject, TIdent>
    where TDomainObject : class;

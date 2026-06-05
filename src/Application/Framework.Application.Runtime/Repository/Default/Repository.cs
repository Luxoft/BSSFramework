using Anch.SecuritySystem.AccessDenied;
using Anch.SecuritySystem.Providers;

using Framework.Application.Repository.Generic;
using Framework.Database;

namespace Framework.Application.Repository.Default;

public class Repository<TDomainObject>(
    IAsyncDal<TDomainObject, Guid> dal,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    ISecurityProvider<TDomainObject> securityProvider)
    : GenericRepository<TDomainObject, Guid>(dal, accessDeniedExceptionService, securityProvider),
      IRepository<TDomainObject>
    where TDomainObject : class;


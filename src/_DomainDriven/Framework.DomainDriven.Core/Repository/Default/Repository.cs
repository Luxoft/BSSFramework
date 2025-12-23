using SecuritySystem.AccessDenied;
using SecuritySystem.Providers;

namespace Framework.DomainDriven.Repository;

public class Repository<TDomainObject>(
    IAsyncDal<TDomainObject, Guid> dal,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    ISecurityProvider<TDomainObject> securityProvider)
    : GenericRepository<TDomainObject, Guid>(dal, accessDeniedExceptionService, securityProvider),
      IRepository<TDomainObject>
    where TDomainObject : class;

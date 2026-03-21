using Framework.Application.DAL;

using SecuritySystem;
using SecuritySystem.AccessDenied;
using SecuritySystem.DomainServices;

namespace Framework.Application.Repository.Default;

public class ViewRepository<TDomainObject>(
    IAsyncDal<TDomainObject, Guid> dal,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IDomainSecurityService<TDomainObject> domainSecurityService)
    : Repository<TDomainObject>(
        dal,
        accessDeniedExceptionService,
        domainSecurityService.GetSecurityProvider(SecurityRule.View))
    where TDomainObject : class;

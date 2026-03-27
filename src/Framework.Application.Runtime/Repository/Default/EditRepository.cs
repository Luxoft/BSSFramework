using Framework.Database;

using SecuritySystem;
using SecuritySystem.AccessDenied;
using SecuritySystem.DomainServices;

namespace Framework.Application.Repository.Default;

public class EditRepository<TDomainObject>(
    IAsyncDal<TDomainObject, Guid> dal,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IDomainSecurityService<TDomainObject> domainSecurityService)
    : Repository<TDomainObject>(
        dal,
        accessDeniedExceptionService,
        domainSecurityService.GetSecurityProvider(SecurityRule.Edit))
    where TDomainObject : class;

using Framework.Database;

using Anch.SecuritySystem;
using Anch.SecuritySystem.AccessDenied;
using Anch.SecuritySystem.DomainServices;

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

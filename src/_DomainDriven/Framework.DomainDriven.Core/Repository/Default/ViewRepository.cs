using Framework.SecuritySystem;

namespace Framework.DomainDriven.Repository;

public class ViewRepository<TDomainObject>(
    IAsyncDal<TDomainObject, Guid> dal,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IDomainSecurityService<TDomainObject> domainSecurityService)
    : Repository<TDomainObject>(
        dal,
        accessDeniedExceptionService,
        domainSecurityService.GetSecurityProvider(SecurityRule.View))
    where TDomainObject : class;

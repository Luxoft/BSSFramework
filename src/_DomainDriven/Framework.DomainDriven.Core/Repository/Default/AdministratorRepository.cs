#nullable enable
using Framework.SecuritySystem;

using nuSpec.Abstraction;

namespace Framework.DomainDriven.Repository;

public class AdministratorRepository<TDomainObject>(
    IAsyncDal<TDomainObject, Guid> dal,
    ISpecificationEvaluator specificationEvaluator,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IDomainSecurityService<TDomainObject> domainSecurityService)
    : Repository<TDomainObject>(
        dal,
        specificationEvaluator,
        accessDeniedExceptionService,
        domainSecurityService.GetSecurityProvider(SecurityRole.Administrator))
    where TDomainObject : class;

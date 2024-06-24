#nullable enable
using Framework.SecuritySystem;

using nuSpec.Abstraction;

namespace Framework.DomainDriven.Repository;

public class ViewRepository<TDomainObject> : Repository<TDomainObject>
    where TDomainObject : class
{
    public ViewRepository(
        IAsyncDal<TDomainObject, Guid> dal,
        ISpecificationEvaluator specificationEvaluator,
        IAccessDeniedExceptionService accessDeniedExceptionService,
        IDomainSecurityService<TDomainObject> domainSecurityService)
        : base(dal, specificationEvaluator, accessDeniedExceptionService, domainSecurityService.GetSecurityProvider(SecurityRule.View))
    {
    }
}

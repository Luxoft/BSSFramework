#nullable enable
using Framework.SecuritySystem;

using nuSpec.Abstraction;

namespace Framework.DomainDriven.Repository;

public class EditRepository<TDomainObject> : Repository<TDomainObject>
    where TDomainObject : class
{
    public EditRepository(
        IAsyncDal<TDomainObject, Guid> dal,
        ISpecificationEvaluator specificationEvaluator,
        IAccessDeniedExceptionService accessDeniedExceptionService,
        IDomainSecurityService<TDomainObject> domainSecurityService)
        : base(dal, specificationEvaluator, accessDeniedExceptionService, domainSecurityService.GetSecurityProvider(BLLSecurityMode.Edit))
    {
    }
}

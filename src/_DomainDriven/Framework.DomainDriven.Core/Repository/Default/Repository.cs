#nullable enable

using Framework.SecuritySystem;

using nuSpec.Abstraction;

namespace Framework.DomainDriven.Repository;

public class Repository<TDomainObject> : GenericRepository<TDomainObject, Guid>, IRepository<TDomainObject>
    where TDomainObject : class
{
    public Repository(
        IAsyncDal<TDomainObject, Guid> dal,
        ISpecificationEvaluator specificationEvaluator,
        IAccessDeniedExceptionService accessDeniedExceptionService,
        ISecurityProvider<TDomainObject> securityProvider)
        : base(dal, specificationEvaluator, accessDeniedExceptionService, securityProvider)
    {
    }
}

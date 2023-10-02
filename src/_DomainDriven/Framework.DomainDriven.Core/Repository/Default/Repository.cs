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
        IDisabledSecurityProviderSource disabledSecurityProviderSource,
        ISecurityProvider<TDomainObject>? securityProvider = null)
        : base(dal, specificationEvaluator, accessDeniedExceptionService, disabledSecurityProviderSource, securityProvider)
    {
    }
}

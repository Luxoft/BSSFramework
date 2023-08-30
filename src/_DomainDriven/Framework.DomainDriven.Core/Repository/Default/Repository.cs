#nullable enable

using Framework.SecuritySystem;

using nuSpec.Abstraction;

namespace Framework.DomainDriven.Repository;

public class Repository<TDomainObject> : GenericRepository<TDomainObject, Guid>, IRepository<TDomainObject>
    where TDomainObject : class
{
    public Repository(ISecurityProvider<TDomainObject> securityProvider, IAsyncDal<TDomainObject, Guid> dal, ISpecificationEvaluator specificationEvaluator)
        : base(securityProvider, dal, specificationEvaluator)
    {
    }
}

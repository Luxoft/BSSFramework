using Framework.SecuritySystem;

using nuSpec.Abstraction;

namespace Framework.DomainDriven.Repository;

public class Repository<TDomainObject>(
    IAsyncDal<TDomainObject, Guid> dal,
    ISpecificationEvaluator specificationEvaluator,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    ISecurityProvider<TDomainObject> securityProvider)
    : GenericRepository<TDomainObject, Guid>(dal, specificationEvaluator, accessDeniedExceptionService, securityProvider),
      IRepository<TDomainObject>
    where TDomainObject : class;

﻿#nullable enable
using Framework.SecuritySystem;

using nuSpec.Abstraction;

namespace Framework.DomainDriven.Repository;

public class RemoveRepository<TDomainObject>(
    IAsyncDal<TDomainObject, Guid> dal,
    ISpecificationEvaluator specificationEvaluator,
    IAccessDeniedExceptionService accessDeniedExceptionService,
    IDomainSecurityService<TDomainObject> domainSecurityService)
    : Repository<TDomainObject>(
        dal,
        specificationEvaluator,
        accessDeniedExceptionService,
        domainSecurityService.GetSecurityProvider(SecurityRule.Edit))
    where TDomainObject : class;

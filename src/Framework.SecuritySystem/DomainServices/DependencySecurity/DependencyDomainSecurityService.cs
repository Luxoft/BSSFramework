﻿using Framework.QueryableSource;
using Framework.SecuritySystem.Expanders;

namespace Framework.SecuritySystem;

public class DependencyDomainSecurityService<TDomainObject, TBaseDomainObject>(
    ISecurityRuleExpander securityRuleExpander,
    IDomainSecurityService<TBaseDomainObject> baseDomainSecurityService,
    IQueryableSource queryableSource,
    IRelativeDomainPathInfo<TDomainObject, TBaseDomainObject> pathInfo)
    : DependencyDomainSecurityServiceBase<TDomainObject, TBaseDomainObject>(
        securityRuleExpander,
        baseDomainSecurityService)
    where TBaseDomainObject : class
{
    protected override ISecurityProvider<TDomainObject> CreateDependencySecurityProvider(ISecurityProvider<TBaseDomainObject> baseProvider)
    {
        return new DependencySecurityProvider<TDomainObject, TBaseDomainObject>(
            baseProvider,
            pathInfo.Path,
            queryableSource);
    }
}

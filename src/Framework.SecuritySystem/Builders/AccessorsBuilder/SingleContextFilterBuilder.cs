﻿using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.AccessorsBuilder;

public class SingleContextFilterBuilder<TPermission, TDomainObject, TSecurityContext>(
    IPermissionSystem<TPermission> permissionSystem,
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
    SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath,
    SecurityContextRestriction<TSecurityContext>? securityContextRestriction)
    : ByIdentsFilterBuilder<TPermission, TDomainObject, TSecurityContext>(permissionSystem, hierarchicalObjectExpanderFactory, securityPath, securityContextRestriction)
    where TSecurityContext : class, ISecurityContext
{
    protected override IEnumerable<TSecurityContext> GetSecurityObjects(TDomainObject domainObject)
    {
        var securityObject = securityPath.Expression.Eval(domainObject, LambdaCompileCache);

        if (securityObject != null)
        {
            yield return securityObject;
        }
    }

    private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
}

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.AccessorsBuilder;

public class ManyContextFilterBuilder<TPermission, TDomainObject, TSecurityContext>(
    IPermissionSystem<TPermission> permissionSystem,
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
    SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> securityPath,
    SecurityContextRestrictionFilterInfo<TSecurityContext>? restrictionFilterInfo)
    : ByIdentsFilterBuilder<TPermission, TDomainObject, TSecurityContext>(permissionSystem, hierarchicalObjectExpanderFactory, restrictionFilterInfo)
    where TSecurityContext : class, ISecurityContext
{
    protected override IEnumerable<TSecurityContext> GetSecurityObjects(TDomainObject domainObject) =>
        securityPath.Expression.Eval(domainObject, LambdaCompileCache).EmptyIfNull();

    private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
}

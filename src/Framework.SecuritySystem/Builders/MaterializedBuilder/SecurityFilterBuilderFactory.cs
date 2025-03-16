using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem.Builders._Factory;
using Framework.SecuritySystem.ExternalSystem;
using Framework.SecuritySystem.PermissionOptimization;

namespace Framework.SecuritySystem.Builders.MaterializedBuilder;

public class SecurityFilterBuilderFactory<TDomainObject>(
    IEnumerable<IPermissionSystem> permissionSystems,
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
    IRuntimePermissionOptimizationService permissionOptimizationService) :
    FilterBuilderFactoryBase<TDomainObject, SecurityFilterBuilder<TDomainObject>>,
    ISecurityFilterFactory<TDomainObject>
{
    public SecurityFilterInfo<TDomainObject> CreateFilter(
        DomainSecurityRule.RoleBaseSecurityRule securityRule,
        SecurityPath<TDomainObject> securityPath)
    {
        var securityTypes = securityPath.GetUsedTypes();

        var restrictionFilterInfoList = securityRule.GetSafeSecurityContextRestrictionFilters().ToList();

        var rawPermissions = permissionSystems
                             .SelectMany(ps => ps.GetPermissionSource(securityRule).GetPermissions(securityTypes))
                             .ToList();

        var optimizedPermissions = permissionOptimizationService.Optimize(rawPermissions);

        var expandedPermissions =
            optimizedPermissions.Select(permission => this.TryExpandPermission(permission, securityRule.GetSafeExpandType()));

        var builder = this.CreateBuilder(securityPath, restrictionFilterInfoList);

        var filterExpression = expandedPermissions.BuildOr(builder.GetSecurityFilterExpression);

        var lazyHasAccessFunc = LazyHelper.Create(
            () => filterExpression.UpdateBody(CacheContainsCallVisitor.Value).Compile(LambdaCompileCache));

        return new SecurityFilterInfo<TDomainObject>(
            q => q.Where(filterExpression),
            v => lazyHasAccessFunc.Value(v));
    }

    protected override SecurityFilterBuilder<TDomainObject> CreateBuilder(SecurityPath<TDomainObject>.ConditionPath securityPath)
    {
        return new ConditionFilterBuilder<TDomainObject>(securityPath);
    }

    protected override SecurityFilterBuilder<TDomainObject> CreateBuilder<TSecurityContext>(
        SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath,
        SecurityContextRestrictionFilterInfo<TSecurityContext>? _)
    {
        return new SingleContextFilterBuilder<TDomainObject, TSecurityContext>(securityPath);
    }

    protected override SecurityFilterBuilder<TDomainObject> CreateBuilder<TSecurityContext>(
        SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> securityPath,
        SecurityContextRestrictionFilterInfo<TSecurityContext>? _)
    {
        return new ManyContextFilterBuilder<TDomainObject, TSecurityContext>(securityPath);
    }

    protected override SecurityFilterBuilder<TDomainObject> CreateBuilder(SecurityPath<TDomainObject>.OrSecurityPath securityPath, IReadOnlyList<SecurityContextRestrictionFilterInfo> restrictionFilterInfoList)
    {
        return new OrFilterBuilder<TDomainObject>(this, securityPath, restrictionFilterInfoList);
    }

    protected override SecurityFilterBuilder<TDomainObject> CreateBuilder(SecurityPath<TDomainObject>.AndSecurityPath securityPath, IReadOnlyList<SecurityContextRestrictionFilterInfo> restrictionFilterInfoList)
    {
        return new AndFilterBuilder<TDomainObject>(this, securityPath, restrictionFilterInfoList);
    }

    protected override SecurityFilterBuilder<TDomainObject> CreateBuilder<TNestedObject>(
        SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> securityPath,
        IReadOnlyList<SecurityContextRestrictionFilterInfo> restrictionFilterInfoList)
    {
        var nestedBuilderFactory = new SecurityFilterBuilderFactory<TNestedObject>(
            permissionSystems,
            hierarchicalObjectExpanderFactory,
            permissionOptimizationService);

        return new NestedManyFilterBuilder<TDomainObject, TNestedObject>(nestedBuilderFactory, securityPath, restrictionFilterInfoList);
    }

    private Dictionary<Type, IEnumerable<Guid>> TryExpandPermission(
        Dictionary<Type, List<Guid>> permission,
        HierarchicalExpandType expandType)
    {
        return permission.ToDictionary(
            pair => pair.Key,
            pair => hierarchicalObjectExpanderFactory.Create(pair.Key).Expand(pair.Value, expandType));
    }


    private static readonly ILambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
}

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
    FilterBuilderFactoryBase<TDomainObject, SecurityFilterBuilder<TDomainObject>>, ISecurityFilterFactory<TDomainObject>
{
    public SecurityFilterInfo<TDomainObject> CreateFilter(DomainSecurityRule.RoleBaseSecurityRule securityRule, SecurityPath<TDomainObject> securityPath)
    {
        var securityTypes = securityPath.GetUsedTypes();

        var rawPermissions = permissionSystems
                             .SelectMany(ps => ps.GetPermissionSource(securityRule).GetPermissions(securityTypes))
                             .ToList();

        var optimizedPermissions = permissionOptimizationService.Optimize(rawPermissions);

        var expandedPermissions = optimizedPermissions.Select(permission => this.TryExpandPermission(permission, securityRule.SafeExpandType));

        var builder = this.CreateBuilder(securityPath);

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

    protected override SecurityFilterBuilder<TDomainObject> CreateBuilder<TSecurityContext>(SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath)
    {
        return new SingleContextFilterBuilder<TDomainObject, TSecurityContext>(securityPath);
    }

    protected override SecurityFilterBuilder<TDomainObject> CreateBuilder<TSecurityContext>(SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> securityPath)
    {
        return new ManyContextFilterBuilder<TDomainObject, TSecurityContext>(securityPath);
    }

    protected override SecurityFilterBuilder<TDomainObject> CreateBuilder(SecurityPath<TDomainObject>.OrSecurityPath securityPath)
    {
        return new OrFilterBuilder<TDomainObject>(this, securityPath);
    }

    protected override SecurityFilterBuilder<TDomainObject> CreateBuilder(SecurityPath<TDomainObject>.AndSecurityPath securityPath)
    {
        return new AndFilterBuilder<TDomainObject>(this, securityPath);
    }

    protected override SecurityFilterBuilder<TDomainObject> CreateBuilder<TNestedObject>(SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> securityPath)
    {
        var nestedBuilderFactory = new SecurityFilterBuilderFactory<TNestedObject>(
            permissionSystems,
            hierarchicalObjectExpanderFactory,
            permissionOptimizationService);

        return new NestedManyFilterBuilder<TDomainObject, TNestedObject>(nestedBuilderFactory, securityPath);
    }

    private Dictionary<Type, IEnumerable<Guid>> TryExpandPermission(Dictionary<Type, List<Guid>> permission, HierarchicalExpandType expandType)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        return permission.ToDictionary(
            pair => pair.Key,
            pair => hierarchicalObjectExpanderFactory.Create(pair.Key).Expand(pair.Value, expandType));
    }


    private static readonly ILambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
}

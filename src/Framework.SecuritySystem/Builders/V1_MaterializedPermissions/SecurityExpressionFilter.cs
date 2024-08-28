using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.SecuritySystem.Builders._Filter;

namespace Framework.SecuritySystem.Builders.V1_MaterializedPermissions;

public class SecurityExpressionFilter<TDomainObject> : ISecurityExpressionFilter<TDomainObject>

    where TDomainObject : class, IIdentityObject<Guid>
{
    private readonly SecurityExpressionBuilderBase<TDomainObject> builder;

    private readonly Lazy<Func<TDomainObject, IEnumerable<string>>> getAccessorsFunc;

    private readonly Lazy<Func<TDomainObject, bool>> lazyHasAccessFunc;

    private static readonly ILambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);

    public SecurityExpressionFilter(
        SecurityExpressionBuilderBase<TDomainObject> builder,
        DomainSecurityRule.RoleBaseSecurityRule securityRule,
        IEnumerable<Type> securityTypes)
    {
        this.builder = builder;
        var rawPermissions =
            builder.Factory
                   .PermissionSystems
                   .SelectMany(ps => ps.GetPermissionSource(securityRule).GetPermissions(securityTypes))
                   .ToList();

        var optimizedPermissions = builder.Factory.PermissionOptimizationService.Optimize(rawPermissions);

        var expandedPermissions = optimizedPermissions.Select(permission => this.TryExpandPermission(permission, securityRule.SafeExpandType));

        var filterExpression = expandedPermissions.BuildOr(builder.GetSecurityFilterExpression);

        this.InjectFunc = q => q.Where(filterExpression);

        this.lazyHasAccessFunc = LazyHelper.Create(
            () => filterExpression.UpdateBody(CacheContainsCallVisitor.Value).Compile(LambdaCompileCache));

        this.getAccessorsFunc = LazyHelper.Create(
            () => FuncHelper.Create(
                (TDomainObject domainObject) =>
                {
                    var filter = builder.GetAccessorsFilter(domainObject, securityRule.SafeExpandType);

                    return builder.Factory
                                  .PermissionSystems
                                  .SelectMany(ps => ps.GetPermissionSource(securityRule).GetAccessors(filter))
                                  .Distinct();
                }));
    }

    public Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>> InjectFunc { get; }

    public Func<TDomainObject, bool> HasAccessFunc => this.lazyHasAccessFunc.Value;

    public IEnumerable<string> GetAccessors(TDomainObject domainObject)
    {
        return this.getAccessorsFunc.Value(domainObject);
    }

    private Dictionary<Type, IEnumerable<Guid>> TryExpandPermission(
        Dictionary<Type, List<Guid>> permission,
        HierarchicalExpandType expandType)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        return permission.ToDictionary(
            pair => pair.Key,
            pair => this.builder.Factory.HierarchicalObjectExpanderFactory.Create(pair.Key).Expand(pair.Value, expandType));
    }
}

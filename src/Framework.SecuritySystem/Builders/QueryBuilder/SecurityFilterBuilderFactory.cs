using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem.Builders._Factory;
using Framework.SecuritySystem.ExternalSystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem.Builders.QueryBuilder;

public class SecurityFilterBuilderFactory<TDomainObject>(
    IServiceProvider serviceProvider,
    IEnumerable<IPermissionSystem> permissionSystems) :
    ISecurityFilterFactory<TDomainObject>
{
    public SecurityFilterInfo<TDomainObject> CreateFilter(DomainSecurityRule.RoleBaseSecurityRule securityRule, SecurityPath<TDomainObject> securityPath)
    {
        var securityFilterInfoList = permissionSystems.Select(
            permissionSystem =>
            {
                var factoryType = typeof(SecurityFilterBuilderFactory<,>).MakeGenericType(
                    permissionSystem.PermissionType,
                    typeof(TDomainObject));

                var factory = (ISecurityFilterFactory<TDomainObject>)ActivatorUtilities.CreateInstance(
                    serviceProvider,
                    factoryType,
                    permissionSystem);

                return factory.CreateFilter(securityRule, securityPath);
            }).ToList();

        return new SecurityFilterInfo<TDomainObject>(
            q => securityFilterInfoList
                 .Match(
                     () => q.Where(_ => false),
                     filter => filter.InjectFunc(q),
                     filters => filters.Aggregate(q, (state, filter) => state.Union(filter.InjectFunc(q)))),

            domainObject => securityFilterInfoList.Any(filter => filter.HasAccessFunc(domainObject)));
    }
}

public class SecurityFilterBuilderFactory<TPermission, TDomainObject>(
    IPermissionSystem<TPermission> permissionSystem,
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory) :
    FilterBuilderFactoryBase<TDomainObject, SecurityFilterBuilder<TPermission, TDomainObject>>, ISecurityFilterFactory<TDomainObject>
{
    public SecurityFilterInfo<TDomainObject> CreateFilter(DomainSecurityRule.RoleBaseSecurityRule securityRule, SecurityPath<TDomainObject> securityPath)
    {
        var builder = this.CreateBuilder(securityPath);

        var permissionFilterExpression = builder.GetSecurityFilterExpression(securityRule.GetSafeExpandType()).ExpandConst().InlineEval();

        var permissionQuery = permissionSystem.GetPermissionSource(securityRule).GetPermissionQuery();

        var filterExpression = ExpressionHelper.Create(
                                                   (TDomainObject domainObject) =>
                                                       permissionQuery.Any(
                                                           permission => permissionFilterExpression.Eval(domainObject, permission)))
                                               .ExpandConst().InlineEval();

        var lazyHasAccessFunc = LazyHelper.Create(
            () => filterExpression.UpdateBody(CacheContainsCallVisitor.Value).Compile(LambdaCompileCache));

        return new SecurityFilterInfo<TDomainObject>(
            q => q.Where(filterExpression),
            v => lazyHasAccessFunc.Value(v));
    }

    protected override SecurityFilterBuilder<TPermission, TDomainObject> CreateBuilder(SecurityPath<TDomainObject>.ConditionPath securityPath)
    {
        return new ConditionFilterBuilder<TPermission, TDomainObject>(securityPath);
    }

    protected override SecurityFilterBuilder<TPermission, TDomainObject> CreateBuilder<TSecurityContext>(SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> securityPath)
    {
        return new SingleContextFilterBuilder<TPermission, TDomainObject, TSecurityContext>(permissionSystem, hierarchicalObjectExpanderFactory, securityPath);
    }

    protected override SecurityFilterBuilder<TPermission, TDomainObject> CreateBuilder<TSecurityContext>(SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> securityPath)
    {
        return new ManyContextFilterBuilder<TPermission, TDomainObject, TSecurityContext>(permissionSystem, hierarchicalObjectExpanderFactory, securityPath);
    }

    protected override SecurityFilterBuilder<TPermission, TDomainObject> CreateBuilder(SecurityPath<TDomainObject>.OrSecurityPath securityPath)
    {
        return new OrFilterBuilder<TPermission, TDomainObject>(this, securityPath);
    }

    protected override SecurityFilterBuilder<TPermission, TDomainObject> CreateBuilder(SecurityPath<TDomainObject>.AndSecurityPath securityPath)
    {
        return new AndFilterBuilder<TPermission, TDomainObject>(this, securityPath);
    }

    protected override SecurityFilterBuilder<TPermission, TDomainObject> CreateBuilder<TNestedObject>(SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> securityPath)
    {
        var nestedBuilderFactory = new SecurityFilterBuilderFactory<TPermission, TNestedObject>(
            permissionSystem,
            hierarchicalObjectExpanderFactory);

        return new NestedManyFilterBuilder<TPermission, TDomainObject, TNestedObject>(nestedBuilderFactory, securityPath);
    }

    private static readonly ILambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
}

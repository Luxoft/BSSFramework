using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.SecuritySystem.Builders._Factory;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.QueryBuilder;

public class SecurityFilterBuilderFactory<TDomainObject>(
    IEnumerable<IPermissionSystem> permissionSystems,
    IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory) :
    FilterBuilderFactoryBase<TDomainObject, SecurityFilterBuilder<TDomainObject>>, ISecurityFilterFactory<TDomainObject>
{
    public SecurityFilterInfo<TDomainObject> CreateFilter(DomainSecurityRule.RoleBaseSecurityRule securityRule, SecurityPath<TDomainObject> securityPath)
    {
        var builder = this.CreateBuilder(securityPath);

        var permissionFilterExpression = builder.GetSecurityFilterExpression(securityRule.SafeExpandType).ExpandConst().InlineEval();

        var filterExpression =
            permissionSystems
                .Select(ps => ps.GetPermissionSource(securityRule).GetPermissionQuery())
                .Select(
                    baseQuery =>
                        ExpressionHelper.Create(
                                            (TDomainObject domainObject) =>
                                                baseQuery.Any(permission => permissionFilterExpression.Eval(domainObject, permission)))
                                        .ExpandConst().InlineEval())
                .BuildOr();

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
        return new SingleContextFilterBuilder<TDomainObject, TSecurityContext>(hierarchicalObjectExpanderFactory, securityPath);
    }

    protected override SecurityFilterBuilder<TDomainObject> CreateBuilder<TSecurityContext>(SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> securityPath)
    {
        return new ManyContextFilterBuilder<TDomainObject, TSecurityContext>(hierarchicalObjectExpanderFactory, securityPath);
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
            hierarchicalObjectExpanderFactory);

        return new NestedManyFilterBuilder<TDomainObject, TNestedObject>(nestedBuilderFactory, securityPath);
    }

    private static readonly ILambdaCompileCache LambdaCompileCache = new LambdaCompileCache();
}

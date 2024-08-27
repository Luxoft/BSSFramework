using Framework.Core;
using Framework.Persistent;

namespace Framework.SecuritySystem.Rules.Builders.MaterializedPermissions;

public class SecurityExpressionFilter<TDomainObject, TIdent> : ISecurityExpressionFilter<TDomainObject>

    where TDomainObject : class, IIdentityObject<TIdent>
{
    private readonly Lazy<Func<TDomainObject, IEnumerable<string>>> getAccessorsFunc;

    private readonly Lazy<Func<TDomainObject, bool>> lazyHasAccessFunc;

    private static readonly ILambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);

    public SecurityExpressionFilter(
        SecurityExpressionBuilderBase<TDomainObject, TIdent> builder,
        DomainSecurityRule.RoleBaseSecurityRule securityRule,
        IEnumerable<Type> securityTypes)
    {
        var permissions = builder.Factory.AuthorizationSystem.GetPermissions(securityRule, securityTypes);

        var filterExpression = builder.GetSecurityFilterExpression(permissions);

        this.InjectFunc = q => q.Where(filterExpression);

        this.lazyHasAccessFunc = LazyHelper.Create(
            () => filterExpression.UpdateBody(OptimizeContainsCallVisitor<TIdent>.Value).Compile(LambdaCompileCache));

        this.getAccessorsFunc = LazyHelper.Create(
            () => FuncHelper.Create(
                (TDomainObject domainObject) =>
                {
                    var filter = builder.GetAccessorsFilter(domainObject, securityRule.SafeExpandType);

                    return builder.Factory.AuthorizationSystem.GetAccessors(securityRule, filter);
                }));
    }

    public Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>> InjectFunc { get; }

    public Func<TDomainObject, bool> HasAccessFunc => this.lazyHasAccessFunc.Value;

    public IEnumerable<string> GetAccessors(TDomainObject domainObject)
    {
        return this.getAccessorsFunc.Value(domainObject);
    }
}

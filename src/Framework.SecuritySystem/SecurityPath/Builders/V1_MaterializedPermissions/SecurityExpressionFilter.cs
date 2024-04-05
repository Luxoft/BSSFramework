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
        SecurityOperation securityRule)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        if (securityRule == null) throw new ArgumentNullException(nameof(securityRule));

        var usedTypes = builder.GetUsedTypes().Distinct();

        var permissions = builder.Factory.AuthorizationSystem.GetPermissions(securityRule, usedTypes);

        var filterExpression = builder.GetSecurityFilterExpression(permissions);

        this.InjectFunc = q => q.Where(filterExpression);

        this.lazyHasAccessFunc = LazyHelper.Create(
            () => filterExpression.UpdateBody(OptimizeContainsCallVisitor<TIdent>.Value).Compile(LambdaCompileCache));

        this.getAccessorsFunc = LazyHelper.Create(
            () => FuncHelper.Create(
                (TDomainObject domainObject) =>
                {
                    var baseFilter = builder.GetAccessorsFilterMany(domainObject, securityRule.ExpandType);

                    var filter = baseFilter.OverrideInput((IPrincipal<TIdent> principal) => principal.Permissions);

                    return builder.Factory.AuthorizationSystem.GetNonContextAccessors(securityRule, filter);
                }));
    }

    public Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>> InjectFunc { get; }

    public Func<TDomainObject, bool> HasAccessFunc => this.lazyHasAccessFunc.Value;

    public IEnumerable<string> GetAccessors(TDomainObject domainObject)
    {
        return this.getAccessorsFunc.Value(domainObject);
    }
}

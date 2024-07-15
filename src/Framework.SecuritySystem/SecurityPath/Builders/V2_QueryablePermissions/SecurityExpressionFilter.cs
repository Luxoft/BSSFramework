using System.Diagnostics.CodeAnalysis;

using Framework.Core;
using Framework.Persistent;

namespace Framework.SecuritySystem.Rules.Builders.QueryablePermissions;

public class SecurityExpressionFilter<TDomainObject, TIdent> : ISecurityExpressionFilter<TDomainObject>

    where TDomainObject : class, IIdentityObject<TIdent>
{
    private readonly Lazy<Func<TDomainObject, IEnumerable<string>>> getAccessorsFunc;

    private readonly Lazy<Func<TDomainObject, bool>> lazyHasAccessFunc;

    // ReSharper disable once StaticMemberInGenericType
    [SuppressMessage("SonarQube", "S2743")]
    private static readonly ILambdaCompileCache LambdaCompileCache = new LambdaCompileCache();

    public SecurityExpressionFilter(
        SecurityExpressionBuilderBase<TDomainObject, TIdent> builder,
        SecurityRule.RoleBaseSecurityRule securityRule)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        if (securityRule == null) throw new ArgumentNullException(nameof(securityRule));

        var filterExpression = builder.GetSecurityFilterExpression(securityRule).ExpandConst().InlineEval();

        this.InjectFunc = q => q.Where(filterExpression);

        this.lazyHasAccessFunc = LazyHelper.Create(
            () => filterExpression.UpdateBody(OptimizeContainsCallVisitor<TIdent>.Value).Compile(LambdaCompileCache));

        this.getAccessorsFunc = LazyHelper.Create(
            () => FuncHelper.Create(
                (TDomainObject domainObject) =>
                {
                    var filter = builder.GetAccessorsFilter(domainObject, securityRule.SafeExpandType);

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

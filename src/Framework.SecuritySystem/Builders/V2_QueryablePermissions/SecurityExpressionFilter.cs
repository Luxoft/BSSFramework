using System.Diagnostics.CodeAnalysis;

using Framework.Core;
using Framework.Persistent;
using Framework.SecuritySystem.Builders._Filter;

namespace Framework.SecuritySystem.Builders.V2_QueryablePermissions;

public class SecurityExpressionFilter<TDomainObject> : ISecurityExpressionFilter<TDomainObject>

    where TDomainObject : class, IIdentityObject<Guid>
{
    private readonly Lazy<Func<TDomainObject, IEnumerable<string>>> getAccessorsFunc;

    private readonly Lazy<Func<TDomainObject, bool>> lazyHasAccessFunc;

    // ReSharper disable once StaticMemberInGenericType
    [SuppressMessage("SonarQube", "S2743")]
    private static readonly ILambdaCompileCache LambdaCompileCache = new LambdaCompileCache();

    public SecurityExpressionFilter(
        SecurityExpressionBuilderBase<TDomainObject> builder,
        DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        var filterExpression = builder.GetSecurityFilterExpression(securityRule).ExpandConst().InlineEval();

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
}

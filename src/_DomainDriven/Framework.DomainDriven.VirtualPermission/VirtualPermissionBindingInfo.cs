using System.Linq.Expressions;
using System.Reflection;
using Framework.Core;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.VirtualPermission;

public record VirtualPermissionBindingInfo<TDomainObject>(
    SecurityRole SecurityRole,
    Expression<Func<TDomainObject, string>> PrincipalNamePath,
    IReadOnlyList<LambdaExpression> RestrictionPaths,
    Expression<Func<TDomainObject, bool>> Filter)
{
    public VirtualPermissionBindingInfo(
        SecurityRole securityRole,
        Expression<Func<TDomainObject, string>> principalNamePath)
        : this(securityRole, principalNamePath, [], _ => true)
    {
    }

    public VirtualPermissionBindingInfo<TDomainObject> AddRestriction<TSecurityContext>(
        Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> path)
        where TSecurityContext : ISecurityContext =>

        this with { RestrictionPaths = this.RestrictionPaths.Concat([path]).ToList() };

    public VirtualPermissionBindingInfo<TDomainObject> AddRestriction<TSecurityContext>(
        Expression<Func<TDomainObject, TSecurityContext>> path)
        where TSecurityContext : ISecurityContext =>

        this with { RestrictionPaths = this.RestrictionPaths.Concat([path]).ToList() };

    public VirtualPermissionBindingInfo<TDomainObject> AddFilter(
        Expression<Func<TDomainObject, bool>> filter) =>

        this with { Filter = this.Filter.BuildAnd(filter) };

    public Expression<Func<TDomainObject, IEnumerable<Guid>>> GetRestrictionsExpr(Type securityContextType) =>
        this.GetType().GetMethod(nameof(this.GeRestrictionsExpr), BindingFlags.Instance | BindingFlags.Public, Type.EmptyTypes)!
            .MakeGenericMethod(securityContextType)
            .Invoke<Expression<Func<TDomainObject, IEnumerable<Guid>>>>(this);

    public Expression<Func<TDomainObject, IEnumerable<Guid>>> GeRestrictionsExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid>
    {
        var expressions = this.GetManyRestrictionsExpr<TSecurityContext>();

        return expressions.Match(
            () => _ => new Guid[0],
            single => single,
            many => many.Aggregate(
                (state, expr) => from ids1 in state
                                 from ide2 in expr
                                 select ids1.Concat(ide2)));
    }

    private IEnumerable<Expression<Func<TDomainObject, IEnumerable<Guid>>>> GetManyRestrictionsExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid> =>
        this.RestrictionPaths.Select(restrictionPath => restrictionPath switch
        {
            Expression<Func<TDomainObject, TSecurityContext>> singlePath => singlePath.Select(
                securityContext => securityContext != null ? (IEnumerable<Guid>)new[] { securityContext.Id } : new Guid[0]),

            Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> manyPath => manyPath.Select(
                securityContexts => securityContexts.Select(securityContext => securityContext.Id)),

            _ => throw new InvalidOperationException("invalid path")
        });
}

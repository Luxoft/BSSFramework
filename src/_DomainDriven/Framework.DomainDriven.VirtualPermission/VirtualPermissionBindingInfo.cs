using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.VirtualPermission;

public record VirtualPermissionBindingInfo<TPrincipal, TPermission>(
    SecurityRole SecurityRole,
    Expression<Func<TPermission, TPrincipal>> PrincipalPath,
    Expression<Func<TPrincipal, string>> PrincipalNamePath,
    IReadOnlyList<LambdaExpression> RestrictionPaths,
    Func<IServiceProvider, Expression<Func<TPermission, bool>>> GetFilter,
    Expression<Func<TPermission, Period>>? PeriodFilter = null)
{
    public VirtualPermissionBindingInfo(
        SecurityRole securityRole,
        Expression<Func<TPermission, TPrincipal>> principalPath,
        Expression<Func<TPrincipal, string>> principalNamePath)
        : this(securityRole, principalPath, principalNamePath, [],  _ => _ => true)
    {
    }

    public Guid Id { get; } = Guid.NewGuid();

    public VirtualPermissionBindingInfo<TPrincipal, TPermission> AddRestriction<TSecurityContext>(
        Expression<Func<TPermission, IEnumerable<TSecurityContext>>> path)
        where TSecurityContext : ISecurityContext =>

        this with { RestrictionPaths = this.RestrictionPaths.Concat([path]).ToList() };

    public VirtualPermissionBindingInfo<TPrincipal, TPermission> AddRestriction<TSecurityContext>(
        Expression<Func<TPermission, TSecurityContext>> path)
        where TSecurityContext : ISecurityContext =>

        this with { RestrictionPaths = this.RestrictionPaths.Concat([path]).ToList() };

    public VirtualPermissionBindingInfo<TPrincipal, TPermission> AddFilter(
        Expression<Func<TPermission, bool>> filter) => this.AddFilter(_ => filter);

    public VirtualPermissionBindingInfo<TPrincipal, TPermission> AddFilter(
        Func<IServiceProvider, Expression<Func<TPermission, bool>>> getFilter) =>

        this with { GetFilter = sp => this.GetFilter(sp).BuildAnd(getFilter(sp)) };

    public VirtualPermissionBindingInfo<TPrincipal, TPermission> SetPeriodFilter(
        Expression<Func<TPermission, Period>> periodFilter) =>
    this with { PeriodFilter = periodFilter };

    public IEnumerable<Type> GetSecurityContextTypes()
    {
        return this.RestrictionPaths
                   .Select(restrictionPath => restrictionPath.ReturnType.GetCollectionElementTypeOrSelf())
                   .Distinct();
    }

    public Expression<Func<TPermission, IEnumerable<Guid>>> GetRestrictionsExpr(Type securityContextType) =>
        this.GetType().GetMethod(nameof(this.GetRestrictionsExpr), BindingFlags.Instance | BindingFlags.Public, Type.EmptyTypes)!
            .MakeGenericMethod(securityContextType)
            .Invoke<Expression<Func<TPermission, IEnumerable<Guid>>>>(this);

    public Expression<Func<TPermission, IEnumerable<Guid>>> GetRestrictionsExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext
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

    private IEnumerable<Expression<Func<TPermission, IEnumerable<Guid>>>> GetManyRestrictionsExpr<TSecurityContext>()
        where TSecurityContext : ISecurityContext
    {
        foreach (var restrictionPath in this.RestrictionPaths)
        {
            if (restrictionPath is Expression<Func<TPermission, TSecurityContext>> singlePath)
            {
                yield return singlePath.Select(
                    securityContext => securityContext != null ? (IEnumerable<Guid>)new[] { securityContext.Id } : new Guid[0]);
            }
            else if (restrictionPath is Expression<Func<TPermission, IEnumerable<TSecurityContext>>> manyPath)
            {
                yield return manyPath.Select(securityContexts => securityContexts.Select(securityContext => securityContext.Id));
            }
        }
    }
}

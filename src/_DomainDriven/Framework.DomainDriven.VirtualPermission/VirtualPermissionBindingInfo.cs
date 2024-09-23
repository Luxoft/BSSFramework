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
    Expression<Func<TPermission, bool>> Filter,
    Expression<Func<TPermission, Period>>? PeriodFilter = null)
{
    public VirtualPermissionBindingInfo(
        SecurityRole securityRole,
        Expression<Func<TPermission, TPrincipal>> principalPath,
        Expression<Func<TPrincipal, string>> principalNamePath)
        : this(securityRole, principalPath, principalNamePath, [], _ => true)
    {
    }

    public VirtualPermissionBindingInfo<TPrincipal, TPermission> AddRestriction<TSecurityContext>(
        Expression<Func<TPermission, IEnumerable<TSecurityContext>>> path)
        where TSecurityContext : ISecurityContext =>

        this with { RestrictionPaths = this.RestrictionPaths.Concat([path]).ToList() };

    public VirtualPermissionBindingInfo<TPrincipal, TPermission> AddRestriction<TSecurityContext>(
        Expression<Func<TPermission, TSecurityContext>> path)
        where TSecurityContext : ISecurityContext =>

        this with { RestrictionPaths = this.RestrictionPaths.Concat([path]).ToList() };

    public VirtualPermissionBindingInfo<TPrincipal, TPermission> AddFilter(
        Expression<Func<TPermission, bool>> filter) =>

        this with { Filter = this.Filter.BuildAnd(filter) };

    public VirtualPermissionBindingInfo<TPrincipal, TPermission> SetPeriodFilter(
        Expression<Func<TPermission, Period>> periodFilter) =>
    this with { PeriodFilter = periodFilter };

    public void Validate(ISecurityRoleSource securityRoleSource)
    {
        var securityContextRestrictions = securityRoleSource
                                          .GetSecurityRole(this.SecurityRole)
                                          .Information
                                          .Restriction
                                          .SecurityContextRestrictions;

        if (securityContextRestrictions != null)
        {
            var bindingContextTypes = this.GetSecurityContextTypes().ToList();

            var invalidTypes = bindingContextTypes.Except(securityContextRestrictions.Select(r => r.Type)).ToList();

            if (invalidTypes.Any())
            {
                throw new Exception($"Invalid restriction types: {invalidTypes.Join(", ", t => t.Name)}");
            }

            var missedTypes = securityContextRestrictions
                              .Where(r => r.Required)
                              .Select(r => r.Type)
                              .Except(bindingContextTypes)
                              .ToList();

            if (missedTypes.Any())
            {
                throw new Exception($"Missed required restriction types: {missedTypes.Join(", ", t => t.Name)}");
            }
        }
    }

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

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

    public Expression<Func<TDomainObject, IEnumerable<Guid>>> GetRestrictionsExpr(Type securityContextType) =>
        this.GetType().GetMethod(nameof(this.GetRestrictionsExpr), BindingFlags.Instance | BindingFlags.Public, Type.EmptyTypes)!
            .MakeGenericMethod(securityContextType)
            .Invoke<Expression<Func<TDomainObject, IEnumerable<Guid>>>>(this);

    public Expression<Func<TDomainObject, IEnumerable<Guid>>> GetRestrictionsExpr<TSecurityContext>()
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
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid>
    {
        foreach (var restrictionPath in this.RestrictionPaths)
        {
            if (restrictionPath is Expression<Func<TDomainObject, TSecurityContext>> singlePath)
            {
                yield return singlePath.Select(
                    securityContext => securityContext != null ? (IEnumerable<Guid>)new[] { securityContext.Id } : new Guid[0]);
            }
            else if (restrictionPath is Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> manyPath)
            {
                yield return manyPath.Select(securityContexts => securityContexts.Select(securityContext => securityContext.Id));
            }
        }
    }
}

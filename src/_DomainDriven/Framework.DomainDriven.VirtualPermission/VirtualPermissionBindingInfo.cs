using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.VirtualPermission;

public record VirtualPermissionBindingInfo<TDomainObject>(
    SecurityRole SecurityRole,
    Expression<Func<TDomainObject, string>> PrincipalNamePath,
    IReadOnlyList<LambdaExpression> SecurityContextPaths,
    Expression<Func<TDomainObject, bool>> Filter)
{
    public VirtualPermissionBindingInfo(
        SecurityRole securityRole,
        Expression<Func<TDomainObject, string>> principalNamePath)
        : this(securityRole, principalNamePath, [], _ => true)
    {
    }

    public VirtualPermissionBindingInfo<TDomainObject> AddSecurityContext<TSecurityContext>(
        Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> path)
        where TSecurityContext : ISecurityContext =>

        this with { SecurityContextPaths = this.SecurityContextPaths.Concat([path]).ToList() };

    public VirtualPermissionBindingInfo<TDomainObject> AddSecurityContext<TSecurityContext>(
        Expression<Func<TDomainObject, TSecurityContext>> path)
        where TSecurityContext : ISecurityContext =>

        this with { SecurityContextPaths = this.SecurityContextPaths.Concat([path]).ToList() };

    public VirtualPermissionBindingInfo<TDomainObject> AddFilter(
        Expression<Func<TDomainObject, bool>> filter) =>

        this with { Filter = this.Filter.BuildAnd(filter) };



    public Expression<Func<TDomainObject, IEnumerable<Guid>>> GetPermissionRestrictions(Type securityContextType)
    {
        return this.GetType().GetMethod(nameof(this.GetTypedPermissionRestrictions), BindingFlags.Instance | BindingFlags.NonPublic, true)
                   .MakeGenericMethod(securityContextType)
                   .Invoke<Expression<Func<TDomainObject, IEnumerable<Guid>>>>(this);
    }

    private Expression<Func<TDomainObject, IEnumerable<Guid>>> GetTypedPermissionRestrictions<TSecurityContext>()
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid>
    {
        var expressions = this.GetTypedPermissionRestrictionsInternal<TSecurityContext>();

        return expressions.Match(
            () => _ => new Guid[0],
            single => single,
            many => many.Aggregate(
                (state, expr) => from ids1 in state
                                 from ide2 in expr
                                 select ids1.Concat(ide2)));
    }

    private IEnumerable<Expression<Func<TDomainObject, IEnumerable<Guid>>>> GetTypedPermissionRestrictionsInternal<TSecurityContext>()
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid>
    {
        foreach (var securityContextPath in this.SecurityContextPaths)
        {
            if (securityContextPath is Expression<Func<TDomainObject, TSecurityContext>> singlePath)
            {
                yield return singlePath.Select(securityContext => (IEnumerable<Guid>)new[] { securityContext.Id });
            }
            else if (securityContextPath is Expression<Func<TDomainObject, IEnumerable<TSecurityContext>>> manyPath)
            {
                yield return manyPath.Select(securityContexts => securityContexts.Select(securityContext => securityContext.Id));
            }
        }
    }
}

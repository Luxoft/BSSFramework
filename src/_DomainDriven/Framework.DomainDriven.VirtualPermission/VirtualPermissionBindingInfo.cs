using System.Linq.Expressions;

using Framework.Core;
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
}

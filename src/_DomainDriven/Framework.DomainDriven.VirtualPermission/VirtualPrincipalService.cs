using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.Persistent;
using Framework.QueryableSource;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem.Management;

using NHibernate.Linq;

namespace Framework.DomainDriven.VirtualPermission;

public class VirtualPrincipalService<TPrincipal, TPermission>(
    IQueryableSource queryableSource,
    VirtualPermissionBindingInfo<TPrincipal, TPermission> bindingInfo) : IPrincipalService

    where TPrincipal : IIdentityObject<Guid>
    where TPermission : IIdentityObject<Guid>
{
    public async Task<IEnumerable<TypedPrincipalHeader>> GetPrincipalsAsync(
        string nameFilter,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var principalNamePath = bindingInfo.PrincipalNamePath;

        var toPrincipalAnonHeaderExpr =
            ExpressionHelper
                .Create((TPrincipal principal) => new { principal.Id, Name = principalNamePath.Eval(principal) })
                .InlineEval()
                .ExpandConst();

        var anonHeaders = await queryableSource
                                .GetQueryable<TPermission>()
                                .Where(bindingInfo.Filter)
                                .Select(bindingInfo.PrincipalPath)
                                .Where(
                                    string.IsNullOrWhiteSpace(nameFilter)
                                        ? _ => true
                                        : bindingInfo.PrincipalNamePath.Select(principalName => principalName.Contains(nameFilter)))
                                .OrderBy(bindingInfo.PrincipalNamePath)
                                .Take(limit)
                                .Select(toPrincipalAnonHeaderExpr)
                                .Distinct()
                                .ToListAsync(cancellationToken);

        return anonHeaders.Select(anonHeader => new TypedPrincipalHeader(anonHeader.Id, anonHeader.Name, true));
    }

    public async Task<TypedPrincipal?> TryGetPrincipalAsync(Guid principalId, CancellationToken cancellationToken = default)
    {
        var principal = await queryableSource.GetQueryable<TPrincipal>()
                                             .Where(principal => principal.Id == principalId)
                                             .SingleOrDefaultAsync(cancellationToken);

        if (principal == null)
        {
            return null;
        }
        else
        {
            var header = new TypedPrincipalHeader(principal.Id, bindingInfo.PrincipalNamePath.Eval(principal), true);

            var permissions = await queryableSource.GetQueryable<TPermission>()
                                                   .Where(bindingInfo.Filter)
                                                   .Where(bindingInfo.PrincipalPath.Select(p => p.Id == principalId))
                                                   .ToListAsync(cancellationToken);

            return new TypedPrincipal(header, permissions.Select(this.ToTypedPermission).ToList());
        }
    }

    private TypedPermission ToTypedPermission(TPermission permission)
    {
        var getRestrictionsMethod = this.GetType().GetMethod(nameof(this.GetRestrictions), BindingFlags.Instance | BindingFlags.NonPublic, true);

        var restrictions = bindingInfo
                           .GetSecurityContextTypes()
                           .Select(
                               securityContextType => (securityContextType, getRestrictionsMethod
                                                                            .MakeGenericMethod(securityContextType)
                                                                            .Invoke<IEnumerable<Guid>>(this, permission)
                                                                            .ToReadOnlyListI()))
                           .ToDictionary();

        return new TypedPermission(
            permission.Id,
            bindingInfo.SecurityRole,
            bindingInfo.PeriodFilter?.Eval(permission) ?? Period.Eternity,
            "",
            restrictions,
            true);
    }

    public async Task<IEnumerable<string>> GetLinkedPrincipalsAsync(
        IEnumerable<SecurityRole> securityRoles,
        CancellationToken cancellationToken = default)
    {
        if (securityRoles.Contains(bindingInfo.SecurityRole))
        {
            return await queryableSource.GetQueryable<TPermission>()
                                        .Where(bindingInfo.Filter)
                                        .Select(bindingInfo.PrincipalPath)
                                        .Select(bindingInfo.PrincipalNamePath)
                                        .ToListAsync(cancellationToken);
        }
        else
        {
            return [];
        }
    }

    private IEnumerable<Guid> GetRestrictions<TSecurityContext>(TPermission permission)
        where TSecurityContext : ISecurityContext, IIdentityObject<Guid>
    {
        foreach (var restrictionPath in bindingInfo.RestrictionPaths)
        {
            if (restrictionPath is Expression<Func<TPermission, TSecurityContext>> singlePath)
            {
                var securityContext = singlePath.Eval(permission);

                if (securityContext != null)
                {
                    yield return securityContext.Id;
                }
            }
            else if (restrictionPath is Expression<Func<TPermission, IEnumerable<TSecurityContext>>> manyPath)
            {
                foreach (var securityContext in manyPath.Eval(permission))
                {
                    yield return securityContext.Id;
                }
            }
        }
    }
}

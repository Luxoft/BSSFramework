using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem.Management;

using NHibernate.Linq;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationPrincipalSourceService(
    [DisabledSecurity] IRepository<Principal> principalRepository,
    ISecurityRoleSource securityRoleSource,
    ISecurityContextSource securityContextSource,
    IAvailablePermissionSource availablePermissionSource) : IPrincipalSourceService
{
    public async Task<IEnumerable<TypedPrincipalHeader>> GetPrincipalsAsync(
        string nameFilter,
        int limit,
        CancellationToken cancellationToken = default)
    {
        return await principalRepository.GetQueryable()
                                        .Pipe(
                                            !string.IsNullOrWhiteSpace(nameFilter),
                                            q => q.Where(principal => principal.Name.Contains(nameFilter)))
                                        .Select(principal => new TypedPrincipalHeader(principal.Id, principal.Name, false))
                                        .ToListAsync(cancellationToken);
    }

    public Task<TypedPrincipal?> TryGetPrincipalAsync(string principalName, CancellationToken cancellationToken = default) =>
        this.TryGetPrincipalAsync(principal => principal.Name == principalName, cancellationToken);

    public Task<TypedPrincipal?> TryGetPrincipalAsync(Guid principalId, CancellationToken cancellationToken = default) =>
        this.TryGetPrincipalAsync(principal => principal.Id == principalId, cancellationToken);

    private async Task<TypedPrincipal?> TryGetPrincipalAsync(Expression<Func<Principal, bool>> filter, CancellationToken cancellationToken)
    {
        var principal = await principalRepository.GetQueryable()
                                                 .Where(filter)
                                                 .FetchMany(principal => principal.Permissions)
                                                 .ThenFetch(permission => permission.Restrictions)
                                                 .SingleOrDefaultAsync(cancellationToken);

        if (principal == null)
        {
            return null;
        }
        else
        {
            return new TypedPrincipal(
                new TypedPrincipalHeader(principal.Id, principal.Name, false),
                principal.Permissions
                         .Select(
                             permission => new TypedPermission(
                                 permission.Id,
                                 false,
                                 securityRoleSource.GetSecurityRole(permission.Role.Id),
                                 permission.Period,
                                 permission.Comment,
                                 permission.Restrictions
                                           .GroupBy(r => r.SecurityContextType.Id, r => r.SecurityContextId)
                                           .ToDictionary(
                                               g => securityContextSource.GetSecurityContextInfo(g.Key).Type,
                                               g => g.ToReadOnlyListI())))
                         .ToList());
        }
    }

    public async Task<IEnumerable<string>> GetLinkedPrincipalsAsync(
        IEnumerable<SecurityRole> securityRoles,
        CancellationToken cancellationToken = default)
    {
        return await availablePermissionSource
                     .GetAvailablePermissionsQueryable(
                         DomainSecurityRule.ExpandedRolesSecurityRule.Create(securityRoles) with
                         {
                             CustomCredential = SecurityRuleCredential.AnyUser
                         })
                     .Select(permission => permission.Principal.Name)
                     .Distinct()
                     .ToListAsync(cancellationToken);
    }
}

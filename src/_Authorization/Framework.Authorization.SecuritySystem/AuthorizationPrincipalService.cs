using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem.Management;

using NHibernate.Linq;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationPrincipalService(
    [DisabledSecurity] IRepository<Principal> principalRepository,
    ISecurityRoleSource securityRoleSource,
    ISecurityContextSource securityContextSource,
    IAvailablePermissionSource availablePermissionSource,
    IPrincipalDomainService principalDomainService) : IPrincipalManagementService
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

    public async Task<TypedPrincipal?> TryGetPrincipalAsync(Guid principalId, CancellationToken cancellationToken = default)
    {
        var principal = await principalRepository.GetQueryable()
                                                 .Where(principal => principal.Id == principalId)
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
                                 securityRoleSource.GetSecurityRole(permission.Role.Id),
                                 permission.Period,
                                 permission.Comment,
                                 permission.Restrictions
                                           .GroupBy(r => r.SecurityContextType.Id, r => r.SecurityContextId)
                                           .ToDictionary(
                                               g => securityContextSource.GetSecurityContextInfo(g.Key).Type,
                                               g => g.ToReadOnlyListI()),
                                  false))
                         .ToList());
        }
    }

    public async Task<IEnumerable<string>> GetLinkedPrincipalsAsync(
        IEnumerable<SecurityRole> securityRoles,
        CancellationToken cancellationToken = default)
    {
        return await availablePermissionSource
                     .GetAvailablePermissionsQueryable(DomainSecurityRule.ExpandedRolesSecurityRule.Create(securityRoles), false)
                     .Select(permission => permission.Principal.Name)
                     .Distinct()
                     .ToListAsync(cancellationToken);
    }

    public async Task<Guid> CreatePrincipalAsync(string principalName, CancellationToken cancellationToken = default)
    {
        var principal = await principalDomainService.GetOrCreateAsync(principalName, cancellationToken);

        return principal.Id;
    }

    public async Task UpdatePrincipalNameAsync(Guid principalId, string principalName, CancellationToken cancellationToken)
    {
        var principal = await principalRepository.LoadAsync(principalId, cancellationToken);

        principal.Name = principalName;

        await principalRepository.SaveAsync(principal, cancellationToken);
    }

    public async Task RemovePrincipalAsync(Guid principalId, CancellationToken cancellationToken = default)
    {
        var principal = await principalRepository.LoadAsync(principalId, cancellationToken);

        await principalDomainService.RemoveAsync(principal, cancellationToken);
    }

    public async Task<MergeResult<Guid, Guid>> UpdatePermissionsAsync(
        Guid principalId,
        IEnumerable<TypedPermission> typedPermissions,
        CancellationToken cancellationToken = default)
    {
        var principal = await principalRepository.LoadAsync(principalId, cancellationToken);

        throw new NotImplementedException();
    }
}

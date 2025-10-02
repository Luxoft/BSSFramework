using System.Linq.Expressions;

using CommonFramework;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;

using GenericQueryable;

using SecuritySystem;
using SecuritySystem.Attributes;
using SecuritySystem.Credential;
using SecuritySystem.ExternalSystem.Management;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationPrincipalSourceService(
    [DisabledSecurity] IRepository<Principal> principalRepository,
    ISecurityRoleSource securityRoleSource,
    ISecurityContextInfoSource securityContextInfoSource,
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
                                        .GenericToListAsync(cancellationToken);
    }


    public Task<TypedPrincipal?> TryGetPrincipalAsync(UserCredential userCredential, CancellationToken cancellationToken = default) =>
        userCredential switch
        {
            UserCredential.NamedUserCredential { Name: var principalName } =>
                this.TryGetPrincipalAsync(principal => principal.Name == principalName, cancellationToken),

            UserCredential.IdentUserCredential { Id: var principalId } =>
                this.TryGetPrincipalAsync(principal => principal.Id == principalId, cancellationToken),

            _ => throw new ArgumentOutOfRangeException(nameof(userCredential))
        };

    private async Task<TypedPrincipal?> TryGetPrincipalAsync(Expression<Func<Principal, bool>> filter, CancellationToken cancellationToken)
    {
        var principal = await principalRepository.GetQueryable()
                                                 .Where(filter)
                                                 .WithFetch($"{nameof(Principal.Permissions)}.{nameof(Permission.Restrictions)}")
                                                 //.FetchMany(principal => principal.Permissions)
                                                 //.ThenFetch(permission => permission.Restrictions)
                                                 .GenericSingleOrDefaultAsync(cancellationToken);

        if (principal is null)
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
                                 permission.Period.StartDate,
                                 permission.Period.EndDate,
                                 permission.Comment,
                                 permission.Restrictions
                                           .GroupBy(r => r.SecurityContextType.Id, r => r.SecurityContextId)
                                           .ToDictionary(
                                               g => securityContextInfoSource.GetSecurityContextInfo(g.Key).Type,
                                               Array (g) => g.ToArray())))
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
                             CustomCredential = new SecurityRuleCredential.AnyUserCredential()
                         })
                     .Select(permission => permission.Principal.Name)
                     .Distinct()
                     .GenericToListAsync(cancellationToken);
    }
}

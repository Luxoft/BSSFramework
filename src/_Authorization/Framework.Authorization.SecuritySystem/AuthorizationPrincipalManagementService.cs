using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.Exceptions;
using Framework.Persistent;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem.Management;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationPrincipalManagementService(
    [DisabledSecurity] IRepository<Principal> principalRepository,
    [DisabledSecurity] IRepository<Permission> permissionRepository,
    [DisabledSecurity] IRepository<BusinessRole> businessRoleRepository,
    [DisabledSecurity] IRepository<SecurityContextType> securityContextTypeRepository,
    ISecurityRoleSource securityRoleSource,
    ISecurityContextSource securityContextSource,
    IPrincipalDomainService principalDomainService) : IPrincipalManagementService
{
    public async Task<IIdentityObject<Guid>> CreatePrincipalAsync(string principalName, CancellationToken cancellationToken = default)
    {
        var principal = await principalDomainService.GetOrCreateAsync(principalName, cancellationToken);

        return principal;
    }

    public async Task<IIdentityObject<Guid>> UpdatePrincipalNameAsync(Guid principalId, string principalName, CancellationToken cancellationToken)
    {
        var principal = await principalRepository.LoadAsync(principalId, cancellationToken);

        principal.Name = principalName;

        await principalRepository.SaveAsync(principal, cancellationToken);

        return principal;
    }

    public async Task<IIdentityObject<Guid>> RemovePrincipalAsync(Guid principalId, CancellationToken cancellationToken = default)
    {
        var principal = await principalRepository.LoadAsync(principalId, cancellationToken);

        await principalDomainService.RemoveAsync(principal, cancellationToken);

        return principal;
    }

    public async Task<MergeResult<IIdentityObject<Guid>, IIdentityObject<Guid>>> UpdatePermissionsAsync(
        Guid principalId,
        IEnumerable<TypedPermission> typedPermissions,
        CancellationToken cancellationToken = default)
    {
        var dbPrincipal = await principalRepository.LoadAsync(principalId, cancellationToken);

        var permissionMergeResult = dbPrincipal.Permissions.GetMergeResult(typedPermissions, p => p.Id, p => p.Id);

        var newPermissions = await this.CreatePermissionsAsync(dbPrincipal, permissionMergeResult.AddingItems, cancellationToken);

        var updatedPermissions = await this.UpdatePermissionsAsync(permissionMergeResult.CombineItems, cancellationToken);

        foreach (var oldDbPermission in permissionMergeResult.RemovingItems)
        {
            dbPrincipal.RemoveDetail(oldDbPermission);

            await permissionRepository.RemoveAsync(oldDbPermission, cancellationToken);
        }

        return new MergeResult<IIdentityObject<Guid>, IIdentityObject<Guid>>(
            newPermissions,
            updatedPermissions.Select(pair => (IIdentityObject<Guid>)pair.Item1).Select(v => (v, v)),
            permissionMergeResult.RemovingItems);
    }

    private async Task<IReadOnlyList<Permission>> CreatePermissionsAsync(
        Principal dbPrincipal,
        IEnumerable<TypedPermission> typedPermissions,
        CancellationToken cancellationToken = default)
    {
        return await typedPermissions.SyncWhenAll(
                   typedPermission => this.CreatePermissionAsync(dbPrincipal, typedPermission, cancellationToken));
    }

    private async Task<Permission> CreatePermissionAsync(Principal dbPrincipal, TypedPermission typedPermission, CancellationToken cancellationToken = default)
    {
        if (typedPermission.Id != Guid.Empty || typedPermission.IsVirtual)
        {
            throw new Exception("wrong typed permission");
        }

        var securityRole = securityRoleSource.GetSecurityRole(typedPermission.SecurityRole);

        var dbRole = await businessRoleRepository.LoadAsync(securityRole.Id, cancellationToken);

        var newDbPermission = new Permission(dbPrincipal)
                              {
                                  Comment = typedPermission.Comment,
                                  Period = typedPermission.Period,
                                  Role = dbRole
                              };

        foreach (var restrictionGroup in typedPermission.Restrictions)
        {
            var securityContextTypeId = securityContextSource.GetSecurityContextInfo(restrictionGroup.Key).Id;

            foreach (var securityContextId in restrictionGroup.Value)
            {
                _ = new PermissionRestriction(newDbPermission)
                    {
                        SecurityContextId = securityContextId,
                        SecurityContextType = await securityContextTypeRepository.LoadAsync(
                                                  securityContextTypeId,
                                                  cancellationToken)
                    };
            }
        }

        await permissionRepository.SaveAsync(newDbPermission, cancellationToken);

        return newDbPermission;
    }

    private async Task<IReadOnlyList<(Permission, TypedPermission)>> UpdatePermissionsAsync(IReadOnlyList<(Permission, TypedPermission)> permissionPairs, CancellationToken cancellationToken = default)
    {
        var preResult = await permissionPairs.SyncWhenAll(
                            async permissionPair => new { permissionPair, Updated = await this.UpdatePermission(permissionPair.Item1, permissionPair.Item2, cancellationToken) });

        return preResult
               .Where(pair => pair.Updated)
               .Select(pair => pair.permissionPair)
               .ToList();
    }

    private async Task<bool> UpdatePermission(Permission dbPermission, TypedPermission typedPermission, CancellationToken cancellationToken)
    {
        if (securityRoleSource.GetSecurityRole(dbPermission.Role.Id) != typedPermission.SecurityRole)
        {
            throw new BusinessLogicException("Permission role can't be changed");
        }

        var restrictionMergeResult = dbPermission.Restrictions.GetMergeResult(
            typedPermission.Restrictions.ChangeKey(t => securityContextSource.GetSecurityContextInfo(t).Id)
                           .SelectMany(pair => pair.Value.Select(securityContextId => (pair.Key, securityContextId))),
            r => (r.SecurityContextType.Id, r.SecurityContextId),
            pair => pair);

        if (restrictionMergeResult.IsEmpty && dbPermission.Comment == typedPermission.Comment && dbPermission.Period == typedPermission.Period)
        {
            return false;
        }

        foreach (var restriction in restrictionMergeResult.AddingItems)
        {
            _ = new PermissionRestriction(dbPermission)
                {
                    SecurityContextId = restriction.securityContextId,
                    SecurityContextType = await securityContextTypeRepository.LoadAsync(restriction.Key, cancellationToken)
                };
        }

        foreach (var dbRestriction in restrictionMergeResult.RemovingItems)
        {
            dbPermission.RemoveDetail(dbRestriction);
        }

        return true;
    }
}

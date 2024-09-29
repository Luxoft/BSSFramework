using Framework.Core;
using Framework.Persistent;

namespace Framework.SecuritySystem.ExternalSystem.Management;

public class RootPrincipalManagementService(IEnumerable<IPermissionSystem> permissionSystems) : IPrincipalManagementService
{
    private readonly IReadOnlyList<IPrincipalService> principalServices = permissionSystems.Select(ps => ps.PrincipalService).ToList();

    private IPrincipalManagementService PrincipalManagementService =>
        this.principalServices
            .OfType<IPrincipalManagementService>()
            .Single(
                () => new Exception($"{nameof(this.PrincipalManagementService)} not found"),
                () => new Exception($"More one  {nameof(this.PrincipalManagementService)}"));

    public async Task<IEnumerable<TypedPrincipalHeader>> GetPrincipalsAsync(
        string nameFilter,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var preResult = await this.principalServices.SyncWhenAll(ps => ps.GetPrincipalsAsync(nameFilter, limit, cancellationToken));

        return preResult.SelectMany()
                        .GroupBy(header => header with { IsVirtual = false })
                        .Select(g => g.Key with { IsVirtual = g.All(h => h.IsVirtual) })
                        .OrderBy(header => header.IsVirtual)
                        .ThenBy(header => header.Name)
                        .Take(limit);
    }

    public async Task<TypedPrincipal?> TryGetPrincipalAsync(Guid principalId, CancellationToken cancellationToken = default)
    {
        var preResult = await this.principalServices.SyncWhenAll(ps => ps.TryGetPrincipalAsync(principalId, cancellationToken));

        var request = from principal in preResult

                      where principal != null

                      group principal by principal.Header with { IsVirtual = false }

                      into g

                      select new TypedPrincipal(
                          g.Key with { IsVirtual = g.All(p => p.Header.IsVirtual) },
                          g.SelectMany(p => p.Permissions).ToList());

        return request.SingleOrDefault(() => throw new Exception($"More one principal {principalId}"));
    }

    public async Task<IEnumerable<string>> GetLinkedPrincipalsAsync(
        IEnumerable<SecurityRole> securityRoles,
        CancellationToken cancellationToken = default)
    {
        var preResult = await this.principalServices.SyncWhenAll(ps => ps.GetLinkedPrincipalsAsync(securityRoles, cancellationToken));

        return preResult.SelectMany().Distinct();
    }

    public Task<IIdentityObject<Guid>> CreatePrincipalAsync(string principalName, CancellationToken cancellationToken = default) =>
        this.PrincipalManagementService.CreatePrincipalAsync(principalName, cancellationToken);

    public Task<IIdentityObject<Guid>> UpdatePrincipalNameAsync(Guid principalId, string principalName, CancellationToken cancellationToken) =>
        this.PrincipalManagementService.UpdatePrincipalNameAsync(principalId, principalName, cancellationToken);

    public Task<IIdentityObject<Guid>> RemovePrincipalAsync(Guid principalId, CancellationToken cancellationToken = default) =>
        this.PrincipalManagementService.RemovePrincipalAsync(principalId, cancellationToken);

    public Task<MergeResult<IIdentityObject<Guid>, IIdentityObject<Guid>>> UpdatePermissionsAsync(
        Guid principalId,
        IEnumerable<TypedPermission> typedPermissions,
        CancellationToken cancellationToken = default) =>
        this.PrincipalManagementService.UpdatePermissionsAsync(principalId, typedPermissions.Where(tp => !tp.IsVirtual), cancellationToken);
}

using Framework.Core;
using Framework.Exceptions;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem;
using Framework.SecuritySystem.ExternalSystem.Management;

namespace Framework.Configurator.Services;

public class ConfiguratorPrincipalManagementService(IEnumerable<IPermissionSystem> permissionSystems) : IPrincipalManagementService
{
    private readonly IReadOnlyList<IPrincipalService> principalServices = permissionSystems.Select(ps => ps.PrincipalService).ToList();

    private IPrincipalManagementService PrincipalManagementService =>
        this.principalServices
            .OfType<IPrincipalManagementService>()
            .Single(
                () => new BusinessLogicException($"{nameof(this.PrincipalManagementService)} not found"),
                () => new BusinessLogicException($"More one  {nameof(this.PrincipalManagementService)}"));

    public bool IsReadonly => !this.principalServices.OfType<IPrincipalManagementService>().Any();

    public async Task<IEnumerable<TypedPrincipalHeader>> GetPrincipalsAsync(
        string nameFilter,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var preResult =
            await Task.WhenAll(this.principalServices.Select(ps => ps.GetPrincipalsAsync(nameFilter, limit, cancellationToken)));

        return preResult.SelectMany().Take(limit);
    }

    public async Task<TypedPrincipal?> TryGetPrincipalAsync(Guid principalId, CancellationToken cancellationToken = default)
    {
        var preResult = await Task.WhenAll(this.principalServices.Select(ps => ps.TryGetPrincipalAsync(principalId, cancellationToken)));

        return preResult.Where(principal => principal != null).Distinct().SingleOrDefault();
    }

    public async Task<IEnumerable<string>> GetLinkedPrincipalsAsync(
        IEnumerable<SecurityRole> securityRoles,
        CancellationToken cancellationToken = default)
    {
        var preResult = await Task.WhenAll(
                            this.principalServices.Select(ps => ps.GetLinkedPrincipalsAsync(securityRoles, cancellationToken)));

        return preResult.SelectMany().Distinct();
    }

    public Task<Guid> CreatePrincipalAsync(string principalName, CancellationToken cancellationToken = default) =>
        this.PrincipalManagementService.CreatePrincipalAsync(principalName, cancellationToken);

    public Task UpdatePrincipalNameAsync(Guid principalId, string principalName, CancellationToken cancellationToken) =>
        this.PrincipalManagementService.UpdatePrincipalNameAsync(principalId, principalName, cancellationToken);

    public Task RemovePrincipalAsync(Guid principalId, CancellationToken cancellationToken = default) =>
        this.PrincipalManagementService.RemovePrincipalAsync(principalId, cancellationToken);

    public Task<MergeResult<Guid, Guid>> UpdatePermissionsAsync(
        Guid principalId,
        IEnumerable<TypedPermission> typedPermissions,
        CancellationToken cancellationToken = default) =>
        this.PrincipalManagementService.UpdatePermissionsAsync(principalId, typedPermissions, cancellationToken);
}

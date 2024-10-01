using Framework.Core;

namespace Framework.SecuritySystem.ExternalSystem.Management;

public class RootPrincipalSourceService(IEnumerable<IPrincipalSourceService> principalSourceServices) : IRootPrincipalSourceService
{
    public async Task<IEnumerable<TypedPrincipalHeader>> GetPrincipalsAsync(
        string nameFilter,
        int limit,
        CancellationToken cancellationToken = default)
    {
        var preResult = await principalSourceServices.SyncWhenAll(ps => ps.GetPrincipalsAsync(nameFilter, limit, cancellationToken));

        return preResult.SelectMany()
                        .GroupBy(header => header with { IsVirtual = false })
                        .Select(g => g.Key with { IsVirtual = g.All(h => h.IsVirtual) })
                        .OrderBy(header => header.IsVirtual)
                        .ThenBy(header => header.Name)
                        .Take(limit);
    }

    public Task<TypedPrincipal?> TryGetPrincipalAsync(string principalName, CancellationToken cancellationToken = default)
    {
        return this.TryGetPrincipalAsync(
                ps => ps.TryGetPrincipalAsync(principalName, cancellationToken),
                () => throw new Exception($"More one principal {principalName}"));
    }

    public Task<TypedPrincipal?> TryGetPrincipalAsync(Guid principalId, CancellationToken cancellationToken = default)
    {
        return this.TryGetPrincipalAsync(
            ps => ps.TryGetPrincipalAsync(principalId, cancellationToken),
            () => throw new Exception($"More one principal {principalId}"));
    }

    private async Task<TypedPrincipal?> TryGetPrincipalAsync(
        Func<IPrincipalSourceService, Task<TypedPrincipal?>> getMethod,
        Func<Exception> getOverflowException)
    {
        var preResult = await principalSourceServices.SyncWhenAll(getMethod);

        var request = from principal in preResult

                      where principal != null

                      group principal by principal.Header with { IsVirtual = false }

                      into g

                      select new TypedPrincipal(
                          g.Key with { IsVirtual = g.All(p => p.Header.IsVirtual) },
                          g.SelectMany(p => p.Permissions).ToList());

        return request.SingleOrDefault(() => throw getOverflowException());
    }

    public async Task<IEnumerable<string>> GetLinkedPrincipalsAsync(
        IEnumerable<SecurityRole> securityRoles,
        CancellationToken cancellationToken = default)
    {
        var preResult = await principalSourceServices.SyncWhenAll(ps => ps.GetLinkedPrincipalsAsync(securityRoles, cancellationToken));

        return preResult.SelectMany().Distinct();
    }
}

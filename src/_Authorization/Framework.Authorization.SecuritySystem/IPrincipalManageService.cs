using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystem;

public interface IPrincipalManageService
{
    public Task<Principal> GetOrCreateAsync(string name, CancellationToken cancellationToken = default);

    Task SaveAsync(Principal principal, CancellationToken cancellationToken = default);

    Task RemoveAsync(Principal principal, bool force = false, CancellationToken cancellationToken = default);

    Task ValidateAsync(Principal principal, CancellationToken cancellationToken = default);
}

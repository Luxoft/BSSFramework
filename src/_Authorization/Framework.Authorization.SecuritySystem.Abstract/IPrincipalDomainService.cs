using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystemImpl;

public interface IPrincipalDomainService
{
    public Task<Principal> GetOrCreateAsync(string name, CancellationToken cancellationToken = default);

    Task SaveAsync(Principal principal, CancellationToken cancellationToken = default);

    Task RemoveAsync(Principal principal, CancellationToken cancellationToken = default) =>
        this.RemoveAsync(principal, false, cancellationToken);

    Task RemoveAsync(Principal principal, bool force, CancellationToken cancellationToken = default);

    Task ValidateAsync(Principal principal, CancellationToken cancellationToken = default);
}

using Framework.SecuritySystem.Credential;

namespace Framework.SecuritySystem.ExternalSystem.Management;

public interface IPrincipalSourceService
{
    Task<IEnumerable<TypedPrincipalHeader>> GetPrincipalsAsync(string nameFilter, int limit, CancellationToken cancellationToken = default);

    Task<TypedPrincipal?> TryGetPrincipalAsync(UserCredential userCredential, CancellationToken cancellationToken = default);

    Task<IEnumerable<string>> GetLinkedPrincipalsAsync(IEnumerable<SecurityRole> securityRoles, CancellationToken cancellationToken = default);
}

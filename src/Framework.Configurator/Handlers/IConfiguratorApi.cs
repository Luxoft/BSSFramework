using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.Configurator.Handlers;

public interface IConfiguratorApi
{
    Task<IEnumerable<TypedPrincipalHeader>> GetPrincipalsAsync(string nameFilter, int limit, CancellationToken cancellationToken = default);

    Task<TypedPrincipal> GetPrincipalAsync(Guid principalId, CancellationToken cancellationToken = default);

    Task<Guid> CreatePrincipalAsync(string principalName, CancellationToken cancellationToken = default);

    Task UpdatePrincipalNameAsync(Guid principalId, string principalName, CancellationToken cancellationToken);

    Task RemovePrincipalAsync(Guid principalId, CancellationToken cancellationToken = default);

    Task<IEnumerable<string>> GetLinkedPrincipalsAsync(IEnumerable<SecurityRole> securityRoles, CancellationToken cancellationToken = default);

    Task<MergeResult<Guid, Guid>> UpdatePermissionsAsync(Guid principalId, IEnumerable<TypedPermission> typedPermissions, CancellationToken cancellationToken = default);
}

public record TypedPrincipalHeader(string Name, Guid Id);

public record TypedPrincipal(TypedPrincipalHeader Header, IReadOnlyList<TypedPermission> Permissions);

public record TypedPermission(
    Guid Id,
    SecurityRole SecurityRole,
    Period Period,
    string Comment,
    IReadOnlyDictionary<Type, IReadOnlyList<Guid>> Restrictions);

using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystem.DomainServices;

public interface IBusinessRoleDomainService
{
    Task<BusinessRole> GetAdminRole(CancellationToken cancellationToken = default);

    /// <summary>
    /// For initialization
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<BusinessRole> GetOrCreateEmptyAdminRole(CancellationToken cancellationToken = default);
}

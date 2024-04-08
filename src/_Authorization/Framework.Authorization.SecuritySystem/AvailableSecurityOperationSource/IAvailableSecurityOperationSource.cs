using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface IAvailableSecurityOperationSource
{
    Task<List<SecurityRule>> GetAvailableSecurityOperation(CancellationToken cancellationToken = default);
}

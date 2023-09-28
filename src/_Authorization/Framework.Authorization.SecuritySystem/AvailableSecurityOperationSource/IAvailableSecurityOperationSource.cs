using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface IAvailableSecurityOperationSource
{
    Task<List<SecurityOperation>> GetAvailableSecurityOperation(CancellationToken cancellationToken = default);
}

using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem;

public interface IAvailableSecurityOperationSource
{
    Task<List<SecurityOperation>> GetAvailableSecurityOperations(CancellationToken cancellationToken = default);
}

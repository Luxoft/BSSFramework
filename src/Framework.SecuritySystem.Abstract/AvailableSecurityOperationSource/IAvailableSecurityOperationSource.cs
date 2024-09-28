namespace Framework.SecuritySystem;

public interface IAvailableSecurityOperationSource
{
    Task<List<SecurityOperation>> GetAvailableSecurityOperations(CancellationToken cancellationToken = default);
}

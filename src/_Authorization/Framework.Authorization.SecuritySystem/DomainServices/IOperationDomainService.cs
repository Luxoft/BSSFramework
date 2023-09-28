using Framework.Authorization.Domain;

namespace Framework.Authorization.SecuritySystem.DomainServices;

public interface IOperationDomainService
{
    Task RemoveAsync(Operation operation, CancellationToken cancellationToken = default);
}

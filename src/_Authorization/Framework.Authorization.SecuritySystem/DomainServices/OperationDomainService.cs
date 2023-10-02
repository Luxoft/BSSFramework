using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;
using Framework.Persistent;

namespace Framework.Authorization.SecuritySystem.DomainServices;

public class OperationDomainService : IOperationDomainService
{
    private readonly IRepository<Operation> operationRepository;

    public OperationDomainService(IRepository<Operation> operationRepository)
    {
        this.operationRepository = operationRepository;
    }

    public async Task RemoveAsync(Operation operation, CancellationToken cancellationToken)
    {
        foreach (var link in operation.Links)
        {
            link.BusinessRole.RemoveDetail(link);
        }

        await this.operationRepository.RemoveAsync(operation, cancellationToken);
    }
}

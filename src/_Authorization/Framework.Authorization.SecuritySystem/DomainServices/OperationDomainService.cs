using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;
using Framework.Persistent;

namespace Framework.Authorization.SecuritySystem.DomainServices;

public class OperationDomainService : IOperationDomainService
{
    private readonly IRepositoryFactory<Operation> operationRepositoryFactory;

    public OperationDomainService(IRepositoryFactory<Operation> operationRepositoryFactory)
    {
        this.operationRepositoryFactory = operationRepositoryFactory;
    }

    public async Task RemoveAsync(Operation operation, CancellationToken cancellationToken)
    {
        foreach (var link in operation.Links)
        {
            link.BusinessRole.RemoveDetail(link);
        }

        await this.operationRepositoryFactory.Create().RemoveAsync(operation, cancellationToken);
    }
}

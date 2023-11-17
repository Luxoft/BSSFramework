using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;
using Framework.Persistent;
using Framework.SecuritySystem;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem.DomainServices;

public class OperationDomainService : IOperationDomainService
{
    private readonly IRepository<Operation> operationRepository;

    public OperationDomainService([FromKeyedServices(BLLSecurityMode.Disabled)] IRepository<Operation> operationRepository)
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

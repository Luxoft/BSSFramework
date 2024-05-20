using Framework.DomainDriven.Repository;

using MediatR;

using SampleSystem.BLL._Command.CreateClassA.Intergation;
using SampleSystem.Domain.TestForceAbstract;

namespace SampleSystem.BLL._Command.EmployeeAssistantLinked.Intergation;

public record ClassACreatedEventHandler(IRepositoryFactory<ClassA> Repository) : INotificationHandler<ClassACreatedEvent>
{
    public async Task Handle(ClassACreatedEvent request, CancellationToken cancellationToken)
    {
        var repo = this.Repository.Create();
        var classA = await repo.LoadAsync(request.Id, cancellationToken);
        await repo.RemoveAsync(classA, cancellationToken);
    }
}

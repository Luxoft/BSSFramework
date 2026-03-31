using Bss.Platform.Mediation.Abstractions;

using Framework.Application.Repository;

using SampleSystem.BLL._Command.CreateClassA.Integration;
using SampleSystem.Domain.TestForceAbstract;

namespace SampleSystem.BLL._Command.EmployeeAssistantLinked.Integration;

public record ClassACreatedEventHandler(IRepositoryFactory<ClassA> Repository) : INotificationHandler<ClassACreatedEvent>
{
    public async Task Handle(ClassACreatedEvent request, CancellationToken cancellationToken)
    {
        var repo = this.Repository.Create();
        var classA = await repo.LoadAsync(request.Id, cancellationToken);
        await repo.RemoveAsync(classA, cancellationToken);
    }
}

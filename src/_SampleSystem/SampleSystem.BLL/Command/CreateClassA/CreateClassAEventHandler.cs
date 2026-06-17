using Bss.Platform.Events.Abstractions;

using Framework.Application.Repository;

using MediatR;

using SampleSystem.BLL.Command.CreateClassA.Integration;
using SampleSystem.Domain.TestForceAbstract;

namespace SampleSystem.BLL.Command.CreateClassA;

public record CreateClassAEventHandler(IRepositoryFactory<ClassA> Repository, IIntegrationEventPublisher EventPublisher)
    : IRequestHandler<CreateClassAEvent>
{
    public async Task Handle(CreateClassAEvent request, CancellationToken ct)
    {
        var classA = new ClassA { Value = request.Value };
        await this.Repository.Create().SaveAsync(classA, ct);

        await this.EventPublisher.PublishAsync(new ClassACreatedEvent(classA.Id), ct);

        await Task.Delay(10000, ct);
    }
}


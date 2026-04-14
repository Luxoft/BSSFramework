using Bss.Platform.Events.Abstractions;

using Framework.Application.Repository;

using MediatR;

using SampleSystem.BLL.Command.CreateClassA.Intergation;
using SampleSystem.Domain.TestForceAbstract;

namespace SampleSystem.BLL.Command.CreateClassA;

public record CreateClassAEventHandler(IRepositoryFactory<ClassA> Repository, IIntegrationEventPublisher EventPublisher)
    : IRequestHandler<CreateClassAEvent>
{
    public async Task Handle(CreateClassAEvent request, CancellationToken cancellationToken)
    {
        var classA = new ClassA { Value = request.Value };
        await this.Repository.Create().SaveAsync(classA, cancellationToken);

        await this.EventPublisher.PublishAsync(new ClassACreatedEvent(classA.Id), cancellationToken);

        await Task.Delay(10000, cancellationToken);
    }
}

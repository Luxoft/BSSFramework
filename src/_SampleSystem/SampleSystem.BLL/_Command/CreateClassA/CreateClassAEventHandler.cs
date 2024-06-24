using Bss.Platform.Events.Abstractions;

using Framework.DomainDriven.Repository;

using MediatR;

using SampleSystem.BLL._Command.CreateClassA.Intergation;
using SampleSystem.Domain.TestForceAbstract;

namespace SampleSystem.BLL._Command.CreateClassA;

public record CreateClassAEventHandler(IRepositoryFactory<ClassA> Repository, IIntegrationEventPublisher EventPublisher)
    : IRequestHandler<CreateClassAEvent>
{
    public async Task Handle(CreateClassAEvent request, CancellationToken cancellationToken)
    {
        var classA = new ClassA { Value = request.value };
        await this.Repository.Create().SaveAsync(classA, cancellationToken);

        await this.EventPublisher.PublishAsync(new ClassACreatedEvent(classA.Id), cancellationToken);

        await Task.Delay(10000, cancellationToken);
    }
}

using System.Threading;
using System.Threading.Tasks;

using DotNetCore.CAP;

using Framework.Cap.Abstractions;

namespace Framework.Cap.Impl;

public class IntegrationEventBus : IIntegrationEventBus
{
    private readonly ICapPublisher capPublisher;

    public IntegrationEventBus(ICapPublisher capPublisher) => this.capPublisher = capPublisher;

    public async Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken)
        => await this.capPublisher.PublishAsync(@event.GetType().Name, @event, cancellationToken: cancellationToken);

    public void Publish(IntegrationEvent @event)
        => this.capPublisher.Publish(@event.GetType().Name, @event);
}

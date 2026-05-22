using Bss.Platform.Events.Abstractions;

namespace SampleSystem.IntegrationTests._Environment.Utils;

public class TestIntegrationEventPublisher : IIntegrationEventPublisher
{
    public Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken) => Task.CompletedTask;
}

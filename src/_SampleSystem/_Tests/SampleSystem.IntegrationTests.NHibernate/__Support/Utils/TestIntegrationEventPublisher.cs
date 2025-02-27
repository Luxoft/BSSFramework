using Bss.Platform.Events.Abstractions;

namespace SampleSystem.IntegrationTests.Support.Utils;

public class TestIntegrationEventPublisher : IIntegrationEventPublisher
{
    public Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken) => Task.CompletedTask;
}

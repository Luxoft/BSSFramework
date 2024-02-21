using Framework.Cap.Abstractions;

namespace Automation.ServiceEnvironment.Services;

public class IntegrationTestIntegrationEventBus : IIntegrationEventBus
{
    public async Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken)
    {
    }

    public void Publish(IntegrationEvent @event)
    {
    }
}

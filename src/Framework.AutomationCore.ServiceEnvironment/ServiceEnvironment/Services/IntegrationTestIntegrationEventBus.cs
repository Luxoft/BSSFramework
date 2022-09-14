using System.Threading;
using System.Threading.Tasks;
using Framework.Cap.Abstractions;

namespace Automation.ServiceEnvironment.Services;

public class IntegrationTestIntegrationEventBus : IIntegrationEventBus
{
    public Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken) => Task.CompletedTask;

    public void Publish(IntegrationEvent @event)
    {
    }
}
namespace Framework.Cap.Abstractions;

public interface IIntegrationEventBus
{
    Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken);

    void Publish(IntegrationEvent @event);
}

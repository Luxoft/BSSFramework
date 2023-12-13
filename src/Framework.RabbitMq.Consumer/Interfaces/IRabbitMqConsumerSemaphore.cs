namespace Framework.RabbitMq.Consumer.Interfaces;

public interface IRabbitMqConsumerSemaphore
{
    Task<(bool IsSuccess, DateTime? ObtainedAt)> TryObtainAsync(Guid consumerId, CancellationToken cancellationToken);

    Task<bool> TryReleaseAsync(Guid consumerId, CancellationToken cancellationToken);
}


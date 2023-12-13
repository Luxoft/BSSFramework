using Framework.RabbitMq.Consumer.Interfaces;

namespace Framework.RabbitMq.Consumer.Services;

public record RabbitMqMultipleActiveConsumersSemaphore : IRabbitMqConsumerSemaphore
{
    public Task<(bool IsSuccess, DateTime? ObtainedAt)> TryObtainAsync(Guid consumerId, CancellationToken cancellationToken) =>
        Task.FromResult<(bool, DateTime?)>((true, null));

    public Task<bool> TryReleaseAsync(Guid consumerId, CancellationToken cancellationToken) => Task.FromResult(true);
}

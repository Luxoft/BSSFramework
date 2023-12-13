using Framework.RabbitMq.Consumer.Interfaces;

namespace Framework.RabbitMq.Consumer.Services;

public record RabbitMqMultipleActiveConsumersSemaphore : IRabbitMqConsumerSemaphore
{
    public bool TryObtain(Guid consumerId, out DateTime? obtainedAt)
    {
        obtainedAt = null;

        return true;
    }

    public void TryRelease(Guid consumerId)
    {
        // do nothing for single consumer
    }
}

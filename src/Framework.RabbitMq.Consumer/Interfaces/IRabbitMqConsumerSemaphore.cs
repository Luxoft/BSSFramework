namespace Framework.RabbitMq.Consumer.Interfaces;

public interface IRabbitMqConsumerSemaphore
{
    bool TryObtain(Guid consumerId, out DateTime? obtainedAt);

    void TryRelease(Guid consumerId);
}


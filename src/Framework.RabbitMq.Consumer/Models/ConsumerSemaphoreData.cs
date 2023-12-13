namespace Framework.RabbitMq.Consumer.Models;

public struct ConsumerSemaphoreData
{
    public Guid ConsumerId { get; set; }

    public DateTime ObtainedAt { get; set; }
}

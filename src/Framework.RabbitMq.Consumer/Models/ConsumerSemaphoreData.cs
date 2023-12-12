namespace Framework.RabbitMq.Consumer.Models;

public class ConsumerSemaphoreData
{
    public Guid ConsumerId { get; set; }

    public DateTime ObtainedAt { get; set; }
}

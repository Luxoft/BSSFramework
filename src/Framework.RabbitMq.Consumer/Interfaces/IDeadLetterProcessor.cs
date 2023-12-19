using Framework.RabbitMq.Consumer.Enums;

using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Interfaces;

public interface IDeadLetterProcessor
{
    Task<DeadLetterDecision> ProcessAsync(IModel channel, BasicGetResult message, Exception exception);
}

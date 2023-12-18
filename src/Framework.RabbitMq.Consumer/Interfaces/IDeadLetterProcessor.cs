using Framework.RabbitMq.Consumer.Enums;

using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Interfaces;

public interface IDeadLetterProcessor
{
    DeadLetterDecision ProcessDeadLetter(
        IModel channel,
        BasicGetResult message,
        Exception exception);
}

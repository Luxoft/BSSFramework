namespace Framework.RabbitMq.Consumer.Enums;

public enum DeadLetterDecision
{
    Requeue = 1,

    RemoveFromQueue = 2
}

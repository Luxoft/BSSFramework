namespace Framework.Notification;

public class ActualNotificationProcessor(IActualMailMessageProcessor actualMailMessageProcessor) : IActualNotificationProcessor
{
    public Domain.Notification GetActualNotification(Domain.Notification baseNotification) => baseNotification with { Message = actualMailMessageProcessor.GetActualMailMessage(baseNotification.Message) };
}

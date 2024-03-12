namespace Framework.Events;

public class EventOperationSender : IEventOperationSender
{
    private readonly IEnumerable<IEventOperationReceiver> receivers;

    public EventOperationSender(IEnumerable<IEventOperationReceiver> receivers)
    {
        this.receivers = receivers;
    }

    public void Send<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent)
    {
        foreach (var receiver in this.receivers)
        {
            receiver.Receive(domainObject, domainObjectEvent);
        }
    }
}

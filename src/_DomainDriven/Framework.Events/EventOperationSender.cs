namespace Framework.Events;

public class EventOperationSender(IEnumerable<IEventOperationReceiver> receivers) : IEventOperationSender
{
    public void Send<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent)
    {
        foreach (var receiver in receivers)
        {
            receiver.Receive(domainObject, domainObjectEvent);
        }
    }
}

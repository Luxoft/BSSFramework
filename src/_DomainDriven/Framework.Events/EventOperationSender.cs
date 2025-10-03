namespace Framework.Events;

public class EventOperationSender(IEnumerable<IEventOperationReceiver> receivers) : IEventOperationSender
{
    public async Task Send<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent, CancellationToken cancellationToken)
    {
        foreach (var receiver in receivers)
        {
            await receiver.Receive(domainObject, domainObjectEvent, cancellationToken);
        }
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Events;

public class BLLEventOperationSender : IEventOperationSender
{
    private readonly IEnumerable<IEventOperationReceiver> receivers;

    public BLLEventOperationSender([FromKeyedServices("BLL")]IEnumerable<IEventOperationReceiver> receivers)
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

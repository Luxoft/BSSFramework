using Microsoft.Extensions.DependencyInjection;

namespace Framework.Events.Legacy;

public class BLLEventOperationSender([FromKeyedServices("BLL")] IEnumerable<IEventOperationReceiver> receivers) : IEventOperationSender
{
    public async Task Send<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent, CancellationToken cancellationToken)
    {
        foreach (var receiver in receivers)
        {
            await receiver.Receive(domainObject, domainObjectEvent, cancellationToken);
        }
    }
}

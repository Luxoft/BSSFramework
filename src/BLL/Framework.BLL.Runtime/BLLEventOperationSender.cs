using Framework.Application.Events;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.BLL;

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

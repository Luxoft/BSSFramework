namespace Framework.Application.Events;

public class EmptyEventOperationSender : IEventOperationSender
{
    public async Task Send<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent, CancellationToken ct)
        where TDomainObject : class
    {
    }
}

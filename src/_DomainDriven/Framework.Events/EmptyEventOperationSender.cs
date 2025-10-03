namespace Framework.Events;

public class EmptyEventOperationSender : IEventOperationSender
{
    public async Task Send<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent, CancellationToken cancellationToken)
    {
    }
}

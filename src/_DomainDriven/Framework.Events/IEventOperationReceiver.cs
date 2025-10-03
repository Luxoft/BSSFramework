namespace Framework.Events;

public interface IEventOperationReceiver
{
    Task Receive<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent, CancellationToken cancellationToken);
}

namespace Framework.Events;

public interface IEventOperationSender
{
    Task Send<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent, CancellationToken cancellationToken);
}

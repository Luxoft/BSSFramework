namespace Framework.Events;

public interface IEventOperationSender
{
    void Send<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent);
}

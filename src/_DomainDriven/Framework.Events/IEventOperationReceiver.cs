namespace Framework.Events;

public interface IEventOperationReceiver
{
    void Receive<TDomainObject>(TDomainObject domainObject, DomainObjectEvent domainObjectEvent);
}

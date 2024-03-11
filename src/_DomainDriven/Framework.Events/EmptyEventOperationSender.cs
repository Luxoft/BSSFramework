namespace Framework.Events;

public class EmptyEventOperationSender : IEventOperationSender
{
    public void Send<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent)
    {
    }
}

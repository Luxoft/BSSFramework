using Framework.Events;

namespace Framework.DomainDriven.BLL;

public static class OperationEventSenderExtensions
{
    public static void SendEvent<TDomainObject>(this IOperationEventSender<TDomainObject> eventSender, TDomainObject domainObject, EventOperation operation)
            where TDomainObject : class
    {
        eventSender.SendEvent(new DomainOperationEventArgs<TDomainObject>(domainObject, typeof(TDomainObject), operation));
    }
}

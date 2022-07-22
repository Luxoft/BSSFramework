using System;

namespace Framework.DomainDriven.BLL;

public static class OperationEventSenderExtensions
{
    public static void SendEvent<TDomainObject, TOperation>(this IOperationEventSender<TDomainObject, TOperation> eventSender, TDomainObject domainObject, TOperation operation)
            where TDomainObject : class
            where TOperation : struct, Enum
    {
        eventSender.SendEvent(new DomainOperationEventArgs<TDomainObject, TOperation>(domainObject, typeof(TDomainObject), operation));
    }
}

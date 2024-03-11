using Framework.Core;

namespace Framework.Events;

public static class EventOperationSenderExtensions
{
    public static void Send(this IEventOperationSender sender, object domainObject, Type domainObjectType, DomainObjectEvent domainObjectEvent)
    {
        new Action<object, DomainObjectEvent>(sender.Send)
            .CreateGenericMethod(domainObjectType)
            .Invoke(sender, [domainObject, domainObjectEvent]);
    }
}

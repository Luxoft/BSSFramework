using CommonFramework;

namespace Framework.Events;

public static class EventOperationSenderExtensions
{
    public static void Send(this IEventOperationSender sender, object domainObject, Type domainObjectType, EventOperation domainObjectEvent)
    {
        new Action<object, EventOperation>(sender.Send)
            .CreateGenericMethod(domainObjectType)
            .Invoke(sender, [domainObject, domainObjectEvent]);
    }
}

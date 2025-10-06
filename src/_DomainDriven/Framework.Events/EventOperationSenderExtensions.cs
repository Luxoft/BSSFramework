using CommonFramework;

namespace Framework.Events;

public static class EventOperationSenderExtensions
{
    public static async Task Send(this IEventOperationSender sender, object domainObject, Type domainObjectType, EventOperation domainObjectEvent, CancellationToken cancellationToken)
    {
        await new Func<object, EventOperation, CancellationToken, Task>(sender.Send)
              .CreateGenericMethod(domainObjectType)
              .Invoke<Task>(sender, [domainObject, domainObjectEvent]);
    }
}

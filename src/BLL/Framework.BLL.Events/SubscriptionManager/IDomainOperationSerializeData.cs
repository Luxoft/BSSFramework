using Framework.Application.Events;

namespace Framework.BLL.Events.SubscriptionManager;

public interface IDomainOperationSerializeData<out TDomainObject>
{
    TDomainObject DomainObject { get; }

    EventOperation Operation { get; }

    object? CustomSendObject { get; }

    Type DomainObjectType { get; }
}

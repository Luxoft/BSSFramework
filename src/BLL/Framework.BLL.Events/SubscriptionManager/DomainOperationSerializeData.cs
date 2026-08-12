using Framework.Application.Events;

namespace Framework.BLL.Events.SubscriptionManager;

public readonly struct DomainOperationSerializeData<TDomainObject> : IDomainOperationSerializeData<TDomainObject>
{
    public TDomainObject DomainObject { get; init; }

    public EventOperation Operation { get; init; }

    public object? CustomSendObject { get; init; }

    public Type CustomDomainObjectType { get; init; }

    Type IDomainOperationSerializeData<TDomainObject>.DomainObjectType => this.CustomDomainObjectType ?? typeof(TDomainObject);
}

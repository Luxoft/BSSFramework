namespace Framework.Events.Legacy;

public interface IDomainOperationSerializeData<out TDomainObject>
{
    TDomainObject DomainObject { get; }

    EventOperation Operation { get; }

    object? CustomSendObject { get; }

    Type DomainObjectType { get; }
}

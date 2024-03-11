namespace Framework.Events;

public interface IDomainOperationSerializeData<out TDomainObject>
{
    TDomainObject DomainObject { get; }

    DomainObjectEvent Operation { get; }

    object CustomSendObject { get; }

    Type DomainObjectType { get; }
}

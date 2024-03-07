namespace Framework.Events;

public interface IDomainOperationSerializeData<out TDomainObject>
        where TDomainObject : class
{
    TDomainObject DomainObject { get; }

    EventOperation Operation { get; }

    object CustomSendObject { get; }

    Type DomainObjectType { get; }
}

public struct DomainOperationSerializeData<TDomainObject> : IDomainOperationSerializeData<TDomainObject>
        where TDomainObject : class
{
    public TDomainObject DomainObject { get; set; }

    public EventOperation Operation { get; set; }

    public object CustomSendObject { get; set; }

    public Type CustomDomainObjectType { get; set; }

    Type IDomainOperationSerializeData<TDomainObject>.DomainObjectType => this.CustomDomainObjectType ?? typeof(TDomainObject);
}

namespace Framework.Events;

public struct DomainOperationSerializeData<TDomainObject> : IDomainOperationSerializeData<TDomainObject>
{
    public TDomainObject DomainObject { get; set; }

    public EventOperation Operation { get; set; }

    public object CustomSendObject { get; set; }

    public Type CustomDomainObjectType { get; set; }

    Type IDomainOperationSerializeData<TDomainObject>.DomainObjectType => this.CustomDomainObjectType ?? typeof(TDomainObject);
}

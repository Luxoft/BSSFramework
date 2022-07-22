using System;

namespace Framework.Events
{
    public interface IDomainOperationSerializeData<out TDomainObject>
        where TDomainObject : class
    {
        TDomainObject DomainObject { get; }

        Enum Operation { get; }

        object CustomSendObject { get; }

        Type DomainObjectType { get; }
    }

    public interface IDomainOperationSerializeData<out TDomainObject, out TOperation> : IDomainOperationSerializeData<TDomainObject>
        where TDomainObject : class
        where TOperation : struct, Enum
    {
        new TOperation Operation { get; }
    }

    public struct DomainOperationSerializeData<TDomainObject, TOperation> : IDomainOperationSerializeData<TDomainObject, TOperation>
        where TDomainObject : class
        where TOperation: struct, Enum
    {
        public TDomainObject DomainObject { get; set; }

        public TOperation Operation { get; set; }

        public object CustomSendObject { get; set; }

        public Type CustomDomainObjectType { get; set; }

        Type IDomainOperationSerializeData<TDomainObject>.DomainObjectType => this.CustomDomainObjectType ?? typeof(TDomainObject);

        Enum IDomainOperationSerializeData<TDomainObject>.Operation => this.Operation;
    }
}

using System;

namespace Framework.DomainDriven.BLL
{
    public interface IOperationEventSenderContainer<in TPersistentDomainObjectBase>
    {
        OperationEventSender<TDomainObject, BLLBaseOperation> GetEventSender<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase;

        OperationEventSender<TDomainObject, TOperation> GetEventSender<TDomainObject, TOperation>()
            where TDomainObject : class, TPersistentDomainObjectBase
            where TOperation : struct, Enum;
    }
}

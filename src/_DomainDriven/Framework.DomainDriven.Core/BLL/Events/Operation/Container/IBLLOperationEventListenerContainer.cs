using System;

namespace Framework.DomainDriven.BLL
{
    public interface IOperationEventListenerContainer<in TDomainObjectBase>
    {
        OperationEventListener<TDomainObject, BLLBaseOperation> GetEventListener<TDomainObject>()
            where TDomainObject : class, TDomainObjectBase;

        OperationEventListener<TDomainObject, TOperation> GetEventListener<TDomainObject, TOperation>()
            where TDomainObject : class, TDomainObjectBase
            where TOperation : struct, Enum;
    }
}

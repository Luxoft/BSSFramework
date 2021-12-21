using System;

namespace Framework.DomainDriven.BLL
{
    public interface IBLLOperationEventListenerContainer<in TDomainObjectBase>
    {
        BLLOperationEventListener<TDomainObject, BLLBaseOperation> GetEventListener<TDomainObject>()
            where TDomainObject : class, TDomainObjectBase;

        BLLOperationEventListener<TDomainObject, TOperation> GetEventListener<TDomainObject, TOperation>()
            where TDomainObject : class, TDomainObjectBase
            where TOperation : struct, Enum;
    }
}

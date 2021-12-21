using System;

namespace Framework.DomainDriven.BLL
{
    public interface IBLLOperationEventListener<TDomainObject, TOperation> : IForceEventContainer<TDomainObject, TOperation>
        where TDomainObject : class
        where TOperation : struct, Enum
    {
        event EventHandler<DomainOperationEventArgs<TDomainObject, TOperation>> OperationProcessed;
    }
}

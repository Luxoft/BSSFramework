using System;

namespace Framework.DomainDriven.BLL
{
    public interface IOperationEventListener<TDomainObject, TOperation> : IOperationEventSender<TDomainObject, TOperation>
        where TDomainObject : class
        where TOperation : struct, Enum
    {
        event EventHandler<IDomainOperationEventArgs<TDomainObject, TOperation>> OperationProcessed;
    }
}

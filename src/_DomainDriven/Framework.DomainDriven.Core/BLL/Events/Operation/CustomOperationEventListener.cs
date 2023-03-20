using System;
using System.Collections.Generic;

namespace Framework.DomainDriven.BLL;

internal sealed class CustomOperationEventSender<TDomainObject, TOperation> : OperationEventSender<TDomainObject, TOperation>

        where TDomainObject : class
        where TOperation : struct, Enum
{
    public CustomOperationEventSender(IEnumerable<IOperationEventListener<TDomainObject>> eventListeners, Dictionary<Type, Dictionary<Type, OperationEventSender>> cache)
            : base(eventListeners, cache)
    {
    }

    internal override IEnumerable<KeyValuePair<Type, KeyValuePair<Type, OperationEventSender>>> GetOtherEventListeners()
    {
        return this.GetDefaultOtherEventListeners();
    }
}

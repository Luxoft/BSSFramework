using System;
using System.Collections.Generic;

namespace Framework.DomainDriven.BLL
{
    internal sealed class CustomOperationEventListener<TDomainObject, TOperation> : OperationEventListener<TDomainObject, TOperation>

        where TDomainObject : class
        where TOperation : struct, Enum
    {
        public CustomOperationEventListener(IDictionary<Type, IDictionary<Type, OperationEventListener>> cache)
            : base(cache)
        {
        }

        internal override IEnumerable<KeyValuePair<Type, KeyValuePair<Type, OperationEventListener>>> GetOtherEventListeners()
        {
            return this.GetDefaultOtherEventListeners();
        }
    }
}

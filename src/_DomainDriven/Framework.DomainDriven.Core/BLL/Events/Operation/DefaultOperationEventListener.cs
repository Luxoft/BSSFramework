using System;
using System.Collections.Generic;

namespace Framework.DomainDriven.BLL
{
    internal sealed class DefaultOperationEventListener<TDomainObject> : OperationEventListener<TDomainObject, BLLBaseOperation>

        where TDomainObject : class
    {
        public DefaultOperationEventListener(IDictionary<Type, IDictionary<Type, OperationEventListener>> cache)
            : base(cache)
        {
        }

        internal override IEnumerable<KeyValuePair<Type, KeyValuePair<Type, OperationEventListener>>> GetOtherEventListeners()
        {
            return this.GetDefaultOtherEventListeners();
        }
    }
}

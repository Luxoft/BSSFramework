using System;
using System.Collections.Generic;

namespace Framework.DomainDriven.BLL
{
    internal sealed class BLLCustomOperationEventListener<TDomainObject, TOperation> : BLLOperationEventListener<TDomainObject, TOperation>

        where TDomainObject : class
        where TOperation : struct, Enum
    {
        public BLLCustomOperationEventListener(IDictionary<Type, IDictionary<Type, BLLOperationEventListener>> cache)
            : base(cache)
        {
        }

        internal override IEnumerable<KeyValuePair<Type, KeyValuePair<Type, BLLOperationEventListener>>> GetOtherEventListeners()
        {
            return this.GetDefaultOtherEventListeners();
        }
    }
}
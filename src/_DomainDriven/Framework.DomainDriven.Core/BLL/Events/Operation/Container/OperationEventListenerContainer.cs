using System;
using System.Collections.Generic;

using Framework.Core;

namespace Framework.DomainDriven.BLL
{
    public sealed class OperationEventListenerContainer<TDomainObjectBase> : IOperationEventListenerContainer<TDomainObjectBase>
        where TDomainObjectBase : class
    {
        private readonly object _locker = new object();

        private readonly IDictionary<Type, IDictionary<Type, OperationEventListener>> _cache = new Dictionary<Type, IDictionary<Type, OperationEventListener>>();



        public OperationEventListener<TDomainObject, BLLBaseOperation> GetEventListener<TDomainObject>()
             where TDomainObject : class, TDomainObjectBase
        {
            return this.GetEventListener<TDomainObject, BLLBaseOperation>();
        }

        public OperationEventListener<TDomainObject, TOperation> GetEventListener<TDomainObject, TOperation>()
            where TDomainObject : class, TDomainObjectBase
            where TOperation : struct, Enum
        {
            lock (this._locker)
            {
                return this._cache.GetValueOrCreate(typeof(TDomainObject), () => new Dictionary<Type, OperationEventListener>()).Pipe(domainCache =>
                {
                    var lazyBaseEventListener = LazyHelper.Create(() => domainCache.GetValueOrCreate(typeof(BLLBaseOperation), () => new DefaultOperationEventListener<TDomainObject>(this._cache)) as DefaultOperationEventListener<TDomainObject>);

                    if (typeof(TOperation) == typeof(BLLBaseOperation))
                    {
                        return lazyBaseEventListener.Value;
                    }
                    else
                    {
                        return domainCache.GetValueOrCreate(typeof(TOperation), () =>
                            new CustomOperationEventListener<TDomainObject, TOperation>(this._cache));
                    }
                }) as OperationEventListener<TDomainObject, TOperation>;
            }
        }

        #region IBLLOperationEventListenerContainer<TDomainObjectBase> Members

        OperationEventListener<TDomainObject, BLLBaseOperation> IOperationEventListenerContainer<TDomainObjectBase>.GetEventListener<TDomainObject>()
        {
            return this.GetEventListener<TDomainObject>();
        }

        OperationEventListener<TDomainObject, TOperation> IOperationEventListenerContainer<TDomainObjectBase>.GetEventListener<TDomainObject, TOperation>()
        {
            return this.GetEventListener<TDomainObject, TOperation>();
        }

        #endregion
    }
}

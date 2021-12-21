using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

using Framework.Core;

namespace Framework.DomainDriven.BLL
{
    public sealed class BLLOperationEventListenerContainer<TDomainObjectBase> : IBLLOperationEventListenerContainer<TDomainObjectBase>
        where TDomainObjectBase : class
    {
        private readonly object _locker = new object();

        private readonly IDictionary<Type, IDictionary<Type, BLLOperationEventListener>> _cache = new Dictionary<Type, IDictionary<Type, BLLOperationEventListener>>();



        public BLLOperationEventListener<TDomainObject, BLLBaseOperation> GetEventListener<TDomainObject>()
             where TDomainObject : class, TDomainObjectBase
        {
            return this.GetEventListener<TDomainObject, BLLBaseOperation>();
        }

        public BLLOperationEventListener<TDomainObject, TOperation> GetEventListener<TDomainObject, TOperation>()
            where TDomainObject : class, TDomainObjectBase
            where TOperation : struct, Enum
        {
            lock (this._locker)
            {
                return this._cache.GetValueOrCreate(typeof(TDomainObject), () => new Dictionary<Type, BLLOperationEventListener>()).Pipe(domainCache =>
                {
                    var lazyBaseEventListener = LazyHelper.Create(() => domainCache.GetValueOrCreate(typeof(BLLBaseOperation), () => new BLLDefaultOperationEventListener<TDomainObject>(this._cache)) as BLLDefaultOperationEventListener<TDomainObject>);

                    if (typeof(TOperation) == typeof(BLLBaseOperation))
                    {
                        return lazyBaseEventListener.Value;
                    }
                    else
                    {
                        return domainCache.GetValueOrCreate(typeof(TOperation), () =>
                            new BLLCustomOperationEventListener<TDomainObject, TOperation>(this._cache));
                    }
                }) as BLLOperationEventListener<TDomainObject, TOperation>;
            }
        }

        #region IBLLOperationEventListenerContainer<TDomainObjectBase> Members

        BLLOperationEventListener<TDomainObject, BLLBaseOperation> IBLLOperationEventListenerContainer<TDomainObjectBase>.GetEventListener<TDomainObject>()
        {
            return this.GetEventListener<TDomainObject>();
        }

        BLLOperationEventListener<TDomainObject, TOperation> IBLLOperationEventListenerContainer<TDomainObjectBase>.GetEventListener<TDomainObject, TOperation>()
        {
            return this.GetEventListener<TDomainObject, TOperation>();
        }

        #endregion
    }
}
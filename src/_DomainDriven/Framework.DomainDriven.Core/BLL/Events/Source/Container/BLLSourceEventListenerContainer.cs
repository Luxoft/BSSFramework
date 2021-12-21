using System;
using System.Collections.Generic;

using Framework.Core;

namespace Framework.DomainDriven.BLL
{
    public sealed class BLLSourceEventListenerContainer<TPersistentDomainObjectBase> : IBLLSourceEventListenerContainer<TPersistentDomainObjectBase>
        where TPersistentDomainObjectBase : class
    {
        private readonly object _locker = new object();
        private readonly Dictionary<Type, object> _cache = new Dictionary<Type, object>();


        public BLLSourceEventListenerContainer()
        {

        }

        public BLLSourceEventListener<TDomainObject> GetEventListener<TDomainObject>()
            where TDomainObject : TPersistentDomainObjectBase
        {
            lock (this._locker)
            {
                return this._cache.GetValueOrCreate(typeof(TDomainObject), () => new BLLSourceEventListener<TDomainObject>()) as BLLSourceEventListener<TDomainObject>;
            }
        }

        #region IBLLSourceEventListenerContainer<TPersistentDomainObjectBase> Members

        IBLLSourceEventListener<TDomainObject> IBLLSourceEventListenerContainer<TPersistentDomainObjectBase>.GetEventListener<TDomainObject>()
        {
            return this.GetEventListener<TDomainObject>();
        }

        #endregion
    }
}
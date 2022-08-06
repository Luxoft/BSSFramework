using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL
{
    public sealed class OperationEventSenderContainer<TPersistentDomainObjectBase> : IOperationEventSenderContainer<TPersistentDomainObjectBase>
        where TPersistentDomainObjectBase : class
    {
        private readonly IEnumerable<IOperationEventListener<TPersistentDomainObjectBase>> eventListeners;

        private readonly object _locker = new object();

        private readonly Dictionary<Type, Dictionary<Type, OperationEventSender>> _cache = new Dictionary<Type, Dictionary<Type, OperationEventSender>>();


        public OperationEventSenderContainer([NotNull] IEnumerable<IOperationEventListener<TPersistentDomainObjectBase>> eventListeners)
        {
            this.eventListeners = (eventListeners ?? throw new ArgumentNullException(nameof(eventListeners))).ToArray();
        }


        public OperationEventSender<TDomainObject, BLLBaseOperation> GetEventListener<TDomainObject>()
             where TDomainObject : class, TPersistentDomainObjectBase
        {
            return this.GetEventListener<TDomainObject, BLLBaseOperation>();
        }

        public OperationEventSender<TDomainObject, TOperation> GetEventListener<TDomainObject, TOperation>()
            where TDomainObject : class, TPersistentDomainObjectBase
            where TOperation : struct, Enum
        {
            lock (this._locker)
            {
                return this._cache.GetValueOrCreate(typeof(TDomainObject), () => new Dictionary<Type, OperationEventSender>()).Pipe(domainCache =>
                {
                    var lazyBaseEventListener = LazyHelper.Create(() => domainCache.GetValueOrCreate(typeof(BLLBaseOperation), () => new DefaultOperationEventSender<TDomainObject>(this.eventListeners, this._cache)) as DefaultOperationEventSender<TDomainObject>);

                    if (typeof(TOperation) == typeof(BLLBaseOperation))
                    {
                        return lazyBaseEventListener.Value;
                    }
                    else
                    {
                        return domainCache.GetValueOrCreate(typeof(TOperation), () =>
                            new CustomOperationEventSender<TDomainObject, TOperation>(this.eventListeners, this._cache));
                    }
                }) as OperationEventSender<TDomainObject, TOperation>;
            }
        }

        #region IBLLOperationEventListenerContainer<TDomainObjectBase> Members

        OperationEventSender<TDomainObject, BLLBaseOperation> IOperationEventSenderContainer<TPersistentDomainObjectBase>.GetEventSender<TDomainObject>()
        {
            return this.GetEventListener<TDomainObject>();
        }

        OperationEventSender<TDomainObject, TOperation> IOperationEventSenderContainer<TPersistentDomainObjectBase>.GetEventSender<TDomainObject, TOperation>()
        {
            return this.GetEventListener<TDomainObject, TOperation>();
        }

        #endregion
    }
}

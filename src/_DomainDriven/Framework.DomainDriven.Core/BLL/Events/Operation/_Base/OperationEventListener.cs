using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

namespace Framework.DomainDriven.BLL
{
    public abstract class OperationEventListener
    {
        internal abstract IEnumerable<KeyValuePair<Type, KeyValuePair<Type, OperationEventListener>>> GetOtherEventListeners();

        internal abstract IEnumerable<KeyValuePair<Type, KeyValuePair<Type, OperationEventListener>>> GetDefaultOtherEventListeners();
    }

    public abstract class OperationEventListener<TDomainObject, TOperation> : OperationEventListener, IOperationEventListener<TDomainObject, TOperation>
        where TDomainObject : class
        where TOperation : struct, Enum
    {
        protected readonly IDictionary<Type, IDictionary<Type, OperationEventListener>> Cache;


        internal OperationEventListener(IDictionary<Type, IDictionary<Type, OperationEventListener>> cache)
        {
            this.Cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }


        protected void ProcessOtherEventListeners(IDomainOperationEventArgs<TDomainObject, TOperation> eventArgs)
        {
            var processOtherEventListenerMethod = new Action<OperationEventListener<TDomainObject, TOperation>> (eventArgs.ProcessOtherEventListener).Method.GetGenericMethodDefinition();

            foreach (var otherListener in this.GetOtherEventListeners())
            {
                processOtherEventListenerMethod.MakeGenericMethod(typeof(TDomainObject), typeof(TOperation), otherListener.Key, otherListener.Value.Key)
                                               .Invoke(null, new object[] { eventArgs, otherListener.Value.Value });
            }
        }

        internal sealed override IEnumerable<KeyValuePair<Type, KeyValuePair<Type, OperationEventListener>>> GetDefaultOtherEventListeners()
        {
            return from processedDomainTypePair in this.Cache

                   let processedDomainType = processedDomainTypePair.Key

                   where processedDomainType.IsAssignableFrom(typeof(TDomainObject))

                   from listenerPair in processedDomainTypePair.Value

                   where listenerPair.Value != this

                   select processedDomainType.ToKeyValuePair(listenerPair);
        }

        public void SendEvent(IDomainOperationEventArgs<TDomainObject, TOperation> eventArgs)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

            this.SendEvent(eventArgs, true);
        }

        protected internal virtual void SendEvent(IDomainOperationEventArgs<TDomainObject, TOperation> eventArgs, bool isStart)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

            this.OperationProcessed.Maybe(handler => handler(this, eventArgs));

            if (isStart)
            {
                this.ProcessOtherEventListeners(eventArgs);
            }
        }


        public event EventHandler<IDomainOperationEventArgs<TDomainObject, TOperation>> OperationProcessed;


        protected static readonly bool IsImplemented = !typeof (TDomainObject).IsAbstract;
    }
}

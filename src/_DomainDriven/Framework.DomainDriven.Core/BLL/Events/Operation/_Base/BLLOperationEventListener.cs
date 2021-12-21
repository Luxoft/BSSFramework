using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

namespace Framework.DomainDriven.BLL
{
    public abstract class BLLOperationEventListener
    {
        internal abstract IEnumerable<KeyValuePair<Type, KeyValuePair<Type, BLLOperationEventListener>>> GetOtherEventListeners();

        internal abstract IEnumerable<KeyValuePair<Type, KeyValuePair<Type, BLLOperationEventListener>>> GetDefaultOtherEventListeners();
    }

    public abstract class BLLOperationEventListener<TDomainObject, TOperation> : BLLOperationEventListener, IBLLOperationEventListener<TDomainObject, TOperation>
        where TDomainObject : class
        where TOperation : struct, Enum
    {
        protected readonly IDictionary<Type, IDictionary<Type, BLLOperationEventListener>> Cache;


        internal BLLOperationEventListener(IDictionary<Type, IDictionary<Type, BLLOperationEventListener>> cache)
        {
            this.Cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }


        protected void ProcessOtherEventListeners(DomainOperationEventArgs<TDomainObject, TOperation> eventArgs)
        {
            var processOtherEventListenerMethod = new Action<BLLOperationEventListener<TDomainObject, TOperation>> (eventArgs.ProcessOtherEventListener).Method.GetGenericMethodDefinition();

            foreach (var otherListener in this.GetOtherEventListeners())
            {
                processOtherEventListenerMethod.MakeGenericMethod(typeof(TDomainObject), typeof(TOperation), otherListener.Key, otherListener.Value.Key)
                                               .Invoke(null, new object[] { eventArgs, otherListener.Value.Value });
            }
        }

        internal sealed override IEnumerable<KeyValuePair<Type, KeyValuePair<Type, BLLOperationEventListener>>> GetDefaultOtherEventListeners()
        {
            return from processedDomainTypePair in this.Cache

                   let processedDomainType = processedDomainTypePair.Key

                   where processedDomainType.IsAssignableFrom(typeof(TDomainObject))

                   from listenerPair in processedDomainTypePair.Value

                   where listenerPair.Value != this

                   select processedDomainType.ToKeyValuePair(listenerPair);
        }

        /// <inheritdoc />
        public void ForceEvent(TDomainObject domainObject, TOperation operation)
        {
            this.RaiseOperationProcessed(domainObject, operation, null);
        }

        internal void RaiseOperationProcessed(TDomainObject domainObject, TOperation operation, IOperationBLLBase<TDomainObject> bll)
        {
            this.RaiseOperationProcessed(new DomainOperationEventArgs<TDomainObject, TOperation>(domainObject, typeof(TDomainObject), operation, bll));
        }

        protected internal virtual void RaiseOperationProcessed(DomainOperationEventArgs<TDomainObject, TOperation> eventArgs, bool isStart)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

            this.OperationProcessed.Maybe(handler => handler(this, eventArgs));

            if (isStart)
            {
                this.ProcessOtherEventListeners(eventArgs);
            }
        }

        internal void RaiseOperationProcessed(DomainOperationEventArgs<TDomainObject, TOperation> eventArgs)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

            this.RaiseOperationProcessed(eventArgs, true);
        }


        public event EventHandler<DomainOperationEventArgs<TDomainObject, TOperation>> OperationProcessed;


        protected static readonly bool IsImplemented = !typeof (TDomainObject).IsAbstract;
    }

    public static class BLLOperationEventListenerExtensions
    {
        public static void ProcessOtherEventListener<TDomainObject, TOperation, TOtherDomainObject, TOtherOperation>(this DomainOperationEventArgs<TDomainObject, TOperation> eventArgs,  BLLOperationEventListener<TOtherDomainObject, TOtherOperation> otherListener)
            where TOtherOperation : struct, Enum
            where TOtherDomainObject : class
            where TDomainObject : class, TOtherDomainObject
            where TOperation : struct, Enum
        {
            OperationConverter<TOperation, TOtherOperation>.Map.GetMaybeValue(eventArgs.Operation).Match(otherOperation =>
            {
                var newEventArgs = new DomainOperationEventArgs<TOtherDomainObject, TOtherOperation>(eventArgs.DomainObject, typeof(TDomainObject), otherOperation, eventArgs.BLL as IOperationBLLBase<TOtherDomainObject>);

                otherListener.RaiseOperationProcessed(newEventArgs, false);
            });
        }
    }
}

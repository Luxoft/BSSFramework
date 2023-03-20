using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

namespace Framework.DomainDriven.BLL;

public abstract class OperationEventSender
{
    internal abstract IEnumerable<KeyValuePair<Type, KeyValuePair<Type, OperationEventSender>>> GetOtherEventListeners();

    internal abstract IEnumerable<KeyValuePair<Type, KeyValuePair<Type, OperationEventSender>>> GetDefaultOtherEventListeners();
}

public abstract class OperationEventSender<TDomainObject, TOperation> : OperationEventSender, IOperationEventSender<TDomainObject, TOperation>
        where TDomainObject : class
        where TOperation : struct, Enum
{
    private readonly IEnumerable<IOperationEventListener<TDomainObject>> eventListeners;

    protected readonly Dictionary<Type, Dictionary<Type, OperationEventSender>> Cache;


    internal OperationEventSender(IEnumerable<IOperationEventListener<TDomainObject>> eventListeners, Dictionary<Type, Dictionary<Type, OperationEventSender>> cache)
    {
        this.eventListeners = eventListeners;
        this.Cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }


    protected void ProcessOtherEventListeners(IDomainOperationEventArgs<TDomainObject, TOperation> eventArgs)
    {
        var processOtherEventListenerMethod = new Action<OperationEventSender<TDomainObject, TOperation>> (eventArgs.ProcessOtherEventListener).Method.GetGenericMethodDefinition();

        foreach (var otherListener in this.GetOtherEventListeners())
        {
            processOtherEventListenerMethod.MakeGenericMethod(typeof(TDomainObject), typeof(TOperation), otherListener.Key, otherListener.Value.Key)
                                           .Invoke(null, new object[] { eventArgs, otherListener.Value.Value });
        }
    }

    internal sealed override IEnumerable<KeyValuePair<Type, KeyValuePair<Type, OperationEventSender>>> GetDefaultOtherEventListeners()
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

        this.eventListeners.Foreach(eventListener => eventListener.OnFired(eventArgs));

        if (isStart)
        {
            this.ProcessOtherEventListeners(eventArgs);
        }
    }


    protected static readonly bool IsImplemented = !typeof (TDomainObject).IsAbstract;
}

using Framework.Core;

namespace Framework.DomainDriven.BLL;

public sealed class OperationEventSenderContainer<TPersistentDomainObjectBase> : IOperationEventSenderContainer<TPersistentDomainObjectBase>
        where TPersistentDomainObjectBase : class
{
    private readonly IEnumerable<IOperationEventListener<TPersistentDomainObjectBase>> eventListeners;

    private readonly Dictionary<Type, object> cache = new ();


    public OperationEventSenderContainer(IEnumerable<IOperationEventListener<TPersistentDomainObjectBase>> eventListeners)
    {
        this.eventListeners = (eventListeners ?? throw new ArgumentNullException(nameof(eventListeners))).ToArray();
    }


    public OperationEventSender<TDomainObject> GetEventListener<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        return (OperationEventSender<TDomainObject>)this.cache.GetValueOrCreate(
            typeof(TDomainObject),
            () => new OperationEventSender<TDomainObject>(this.eventListeners));
    }

    #region IBLLOperationEventListenerContainer<TDomainObjectBase> Members

    OperationEventSender<TDomainObject> IOperationEventSenderContainer<TPersistentDomainObjectBase>.GetEventSender<TDomainObject>()
    {
        return this.GetEventListener<TDomainObject>();
    }

    #endregion
}

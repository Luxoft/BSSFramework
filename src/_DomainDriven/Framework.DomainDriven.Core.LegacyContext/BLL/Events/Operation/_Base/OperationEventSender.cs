using Framework.Core;

namespace Framework.DomainDriven.BLL;

public class OperationEventSender<TDomainObject> : IOperationEventSender<TDomainObject>
    where TDomainObject : class
{
    private readonly IEnumerable<IOperationEventListener<TDomainObject>> eventListeners;


    internal OperationEventSender(IEnumerable<IOperationEventListener<TDomainObject>> eventListeners)
    {
        this.eventListeners = eventListeners;
    }

    public void SendEvent(IDomainOperationEventArgs<TDomainObject> eventArgs)
    {
        if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

        this.eventListeners.Foreach(eventListener => eventListener.OnFired(eventArgs));
    }
}

using Framework.Events;

namespace Framework.DomainDriven.BLL;

public abstract class OperationBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject> : BLLContextContainer<TBLLContext>, IOperationBLLBase<TDomainObject>

        where TPersistentDomainObjectBase : class

        where TDomainObject : class, TPersistentDomainObjectBase
        where TBLLContext : class, IBLLOperationEventContext<TPersistentDomainObjectBase>
{
    private readonly Lazy<IOperationEventSender<TDomainObject>> _lazyOperationSender;


    protected OperationBLLBase(TBLLContext context)
            : base(context)
    {
        this._lazyOperationSender = new Lazy<IOperationEventSender<TDomainObject>>(() => this.Context.OperationSenders.GetEventSender<TDomainObject>());
    }


    private IOperationEventSender<TDomainObject> OperationSender
    {
        get { return this._lazyOperationSender.Value; }
    }

    public virtual void Save(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.OperationSender.SendEvent(domainObject, EventOperation.Save);
    }

    public virtual void Remove(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.OperationSender.SendEvent(domainObject, EventOperation.Remove);
    }

    protected void RaiseOperationProcessed(TDomainObject domainObject, EventOperation operation)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.OperationSender.SendEvent(domainObject, operation);
    }

    protected void RaiseOperationProcessed(IDomainOperationEventArgs<TDomainObject> eventArgs)
    {
        if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

        this.OperationSender.SendEvent(eventArgs);
    }
}

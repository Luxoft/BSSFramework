using Framework.Events;

namespace Framework.DomainDriven.BLL;

public abstract class OperationBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject> : BLLContextContainer<TBLLContext>, IOperationBLLBase<TDomainObject>

        where TPersistentDomainObjectBase : class

        where TDomainObject : class, TPersistentDomainObjectBase
        where TBLLContext : class, IBLLOperationEventContext
{
    protected OperationBLLBase(TBLLContext context)
        : base(context)
    {
    }

    public virtual void Save(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.Context.OperationSender.Send(domainObject, DomainObjectEvent.Save);
    }

    public virtual void Remove(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.Context.OperationSender.Send(domainObject, DomainObjectEvent.Remove);
    }
}

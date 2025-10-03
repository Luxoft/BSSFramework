using Framework.Events;

namespace Framework.DomainDriven.BLL;

public abstract class OperationBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObject>(TBLLContext context)
    : BLLContextContainer<TBLLContext>(context), IOperationBLLBase<TDomainObject>
    where TPersistentDomainObjectBase : class
    where TDomainObject : class, TPersistentDomainObjectBase
    where TBLLContext : class, IBLLOperationEventContext
{
    public virtual void Save(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.Context.OperationSender.Send(domainObject, EventOperation.Save, CancellationToken.None).GetAwaiter().GetResult();
    }

    public virtual void Remove(TDomainObject domainObject)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        this.Context.OperationSender.Send(domainObject, EventOperation.Remove, CancellationToken.None).GetAwaiter().GetResult();
    }
}

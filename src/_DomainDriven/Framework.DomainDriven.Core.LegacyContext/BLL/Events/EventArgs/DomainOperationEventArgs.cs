using Framework.Events;

namespace Framework.DomainDriven;

public interface IDomainOperationEventArgs<out TDomainObject>
    where TDomainObject : class
{
    TDomainObject DomainObject { get; }

    Type DomainObjectType { get; }

    EventOperation Operation { get; }
}

public class DomainOperationEventArgs<TDomainObject> : EventArgs, IDomainOperationEventArgs<TDomainObject>
    where TDomainObject : class
{
    public DomainOperationEventArgs(TDomainObject domainObject, Type domainObjectType, EventOperation operation)
    {
        this.DomainObject = domainObject ?? throw new ArgumentNullException(nameof(domainObject));
        this.DomainObjectType = domainObjectType ?? throw new ArgumentNullException(nameof(domainObjectType));
        this.Operation = operation;
    }

    public TDomainObject DomainObject { get; }

    public Type DomainObjectType { get; }

    public EventOperation Operation { get; }
}

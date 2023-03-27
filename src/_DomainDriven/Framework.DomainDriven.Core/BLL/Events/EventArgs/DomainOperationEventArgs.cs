namespace Framework.DomainDriven;

public interface IDomainOperationEventArgs<out TDomainObject>
        where TDomainObject : class
{
    TDomainObject DomainObject { get; }

    Type DomainObjectType { get; }

    Enum Operation { get; }
}

public interface IDomainOperationEventArgs<out TDomainObject, out TOperation> : IDomainOperationEventArgs<TDomainObject>
        where TDomainObject : class
        where TOperation : struct, Enum
{
    new TOperation Operation { get; }
}

public class DomainOperationEventArgs<TDomainObject, TOperation> : EventArgs, IDomainOperationEventArgs<TDomainObject, TOperation>
        where TDomainObject : class
        where TOperation : struct, Enum
{
    public DomainOperationEventArgs(TDomainObject domainObject, Type domainObjectType, TOperation operation)
    {
        this.DomainObject = domainObject ?? throw new ArgumentNullException(nameof(domainObject));
        this.DomainObjectType = domainObjectType ?? throw new ArgumentNullException(nameof(domainObjectType));
        this.Operation = operation;
    }

    public TDomainObject DomainObject { get; }

    public Type DomainObjectType { get; }

    public TOperation Operation { get; }

    Enum IDomainOperationEventArgs<TDomainObject>.Operation => this.Operation;
}

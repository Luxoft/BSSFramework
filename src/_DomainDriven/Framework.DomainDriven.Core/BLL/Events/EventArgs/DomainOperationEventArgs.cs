using System;

namespace Framework.DomainDriven.BLL
{
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
        public DomainOperationEventArgs(TDomainObject domainObject, Type domainObjectType, TOperation operation, IOperationBLLBase<TDomainObject> bll)
        {
            this.DomainObject = domainObject ?? throw new ArgumentNullException(nameof(domainObject));
            this.DomainObjectType = domainObjectType ?? throw new ArgumentNullException(nameof(domainObjectType));
            this.Operation = operation;
            this.BLL = bll;
        }

        public TDomainObject DomainObject { get; }

        public Type DomainObjectType { get; }

        public TOperation Operation { get; }

        [Obsolete("Свойство больше не требуется, так как Listener-ы теперь хранятся рядом с контектом и там всегда есть возможность получить BLL из 'живого' контекста. В 7.0 свойство будет убрано.")]
        public IOperationBLLBase<TDomainObject> BLL { get; }

        Enum IDomainOperationEventArgs<TDomainObject>.Operation => (Enum)(object)this.Operation;
    }
}

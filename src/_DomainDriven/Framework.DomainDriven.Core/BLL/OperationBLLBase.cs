using System;

namespace Framework.DomainDriven.BLL
{
    public abstract class OperationBLLBase<TBLLContext, TPersistentDomainObjectBase, TDomainObjectBase, TDomainObject, TOperation> : BLLContextContainer<TBLLContext>, IOperationBLLBase<TDomainObject>

        where TPersistentDomainObjectBase : class, TDomainObjectBase
        where TDomainObjectBase : class

        where TDomainObject : class, TPersistentDomainObjectBase
        where TOperation : struct, Enum
        where TBLLContext : class, IBLLOperationEventContext<TPersistentDomainObjectBase>
    {
        private readonly Lazy<IOperationEventSender<TDomainObject, TOperation>> _lazyOperationSender;

        private readonly Lazy<IOperationEventSender<TDomainObject, BLLBaseOperation>> _lazyDefaultOperationSender;


        protected OperationBLLBase(TBLLContext context)
            : base(context)
        {
            this._lazyOperationSender = new Lazy<IOperationEventSender<TDomainObject, TOperation>>(() => this.Context.OperationSenders.GetEventSender<TDomainObject, TOperation>());
            this._lazyDefaultOperationSender = new Lazy<IOperationEventSender<TDomainObject, BLLBaseOperation>>(() => this.Context.OperationSenders.GetEventSender<TDomainObject, BLLBaseOperation>());
        }


        private IOperationEventSender<TDomainObject, TOperation> OperationSender
        {
            get { return this._lazyOperationSender.Value; }
        }


        private IOperationEventSender<TDomainObject, BLLBaseOperation> DefaultOperationSender
        {
            get { return this._lazyDefaultOperationSender.Value; }
        }


        public virtual void Save(TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            this.DefaultOperationSender.SendEvent(domainObject, BLLBaseOperation.Save);
        }

        public virtual void Remove(TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            this.DefaultOperationSender.SendEvent(domainObject, BLLBaseOperation.Remove);
        }

        protected void RaiseOperationProcessed(TDomainObject domainObject, TOperation operation)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            this.OperationSender.SendEvent(domainObject, operation);
        }

        protected void RaiseOperationProcessed(IDomainOperationEventArgs<TDomainObject, TOperation> eventArgs)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

            this.OperationSender.SendEvent(eventArgs);
        }
    }
}

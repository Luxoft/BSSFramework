using System;
using Framework.Core;

namespace Framework.DomainDriven.BLL
{
    public abstract class OperationBLLBase<TBLLContext, TDomainObjectBase, TDomainObject, TOperation> : BLLContextContainer<TBLLContext>, IOperationBLLBase<TDomainObject>
        where TDomainObjectBase : class
        where TDomainObject : class, TDomainObjectBase
        where TOperation : struct, Enum
        where TBLLContext : class, IBLLOperationEventContext<TDomainObjectBase>
    {
        private readonly Lazy<IOperationEventSender<TDomainObject, TOperation>> _lazyOperationListener;

        private readonly Lazy<IOperationEventSender<TDomainObject, BLLBaseOperation>> _lazyDefaultOperationListener;


        protected OperationBLLBase(TBLLContext context)
            : base(context)
        {
            this._lazyOperationListener = new Lazy<IOperationEventSender<TDomainObject, TOperation>>(() => this.Context.OperationListeners.GetEventListener<TDomainObject, TOperation>());
            this._lazyDefaultOperationListener = new Lazy<IOperationEventSender<TDomainObject, BLLBaseOperation>>(() => this.Context.OperationListeners.GetEventListener<TDomainObject, BLLBaseOperation>());
        }


        private IOperationEventSender<TDomainObject, TOperation> OperationListener
        {
            get { return this._lazyOperationListener.Value; }
        }


        private IOperationEventSender<TDomainObject, BLLBaseOperation> DefaultOperationListener
        {
            get { return this._lazyDefaultOperationListener.Value; }
        }


        public virtual void Save(TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            this.DefaultOperationListener.SendEvent(domainObject, BLLBaseOperation.Save);
        }

        public virtual void Remove(TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            this.DefaultOperationListener.SendEvent(domainObject, BLLBaseOperation.Remove);
        }

        protected void RaiseOperationProcessed(TDomainObject domainObject, TOperation operation)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            this.OperationListener.SendEvent(domainObject, operation);
        }

        protected void RaiseOperationProcessed(IDomainOperationEventArgs<TDomainObject, TOperation> eventArgs)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

            this.OperationListener.SendEvent(eventArgs);
        }
    }
}

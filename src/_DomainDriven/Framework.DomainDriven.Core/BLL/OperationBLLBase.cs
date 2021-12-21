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
        private readonly Lazy<BLLOperationEventListener<TDomainObject, TOperation>> _lazyOperationListener;

        private readonly Lazy<BLLOperationEventListener<TDomainObject, BLLBaseOperation>> _lazyDefaultOperationListener;


        protected OperationBLLBase(TBLLContext context)
            : base(context)
        {
            this._lazyOperationListener = LazyHelper.Create(() => this.Context.OperationListeners.GetEventListener<TDomainObject, TOperation>());
            this._lazyDefaultOperationListener = LazyHelper.Create(() => this.Context.OperationListeners.GetEventListener<TDomainObject, BLLBaseOperation>());
        }


        private BLLOperationEventListener<TDomainObject, TOperation> OperationListener
        {
            get { return this._lazyOperationListener.Value; }
        }


        private BLLOperationEventListener<TDomainObject, BLLBaseOperation> DefaultOperationListener
        {
            get { return this._lazyDefaultOperationListener.Value; }
        }


        public virtual void Save(TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            this.DefaultOperationListener.RaiseOperationProcessed(domainObject, BLLBaseOperation.Save, this);
        }

        public virtual void Remove(TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            this.DefaultOperationListener.RaiseOperationProcessed(domainObject, BLLBaseOperation.Remove, this);
        }

        protected void RaiseOperationProcessed(TDomainObject domainObject, TOperation operation)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            this.OperationListener.RaiseOperationProcessed(domainObject, operation, this);
        }

        protected void RaiseOperationProcessed(DomainOperationEventArgs<TDomainObject, TOperation> eventArgs)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

            this.OperationListener.RaiseOperationProcessed(eventArgs);
        }
    }
}
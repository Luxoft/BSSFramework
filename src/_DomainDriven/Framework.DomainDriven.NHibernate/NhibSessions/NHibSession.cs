using System;
using System.Collections.Generic;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Tracking;
using Framework.DomainDriven.DAL.Revisions;
using Framework.Persistent;

using JetBrains.Annotations;

using NHibernate;
using NHibernate.Envers.Patch;

namespace Framework.DomainDriven.NHibernate
{
    public abstract class NHibSession : IDBSession
    {
        private Lazy<IAuditReaderPatched> lazyAuditReader { get; }

        internal NHibSession(NHibSessionFactory sessionFactory, DBSessionMode sessionMode)
        {
            this.SessionFactory = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
            this.SessionMode = sessionMode;

            this.lazyAuditReader = LazyHelper.Create(() => this.InnerSession.GetAuditReader());


            this.InnerSession = this.SessionFactory.InternalSessionFactory.OpenSession();
            this.InnerSession.FlushMode = FlushMode.Manual;
        }

        public abstract DBSessionMode Mode { get; }

        public IAuditReaderPatched AuditReader => this.lazyAuditReader.Value;

        public ISession InnerSession { get; }

        protected internal NHibSessionFactory SessionFactory { get; }

        protected internal DBSessionMode SessionMode { get; }

        protected bool HasFlushedListeners => this.Flushed != null;

        public abstract void RegisterModified<TDomainObject>(TDomainObject domainObject, ModificationType modificationType);

        /// <inheritdoc />
        public abstract void Flush();

        /// <inheritdoc />
        public long GetCurrentRevision()
        {
            return this.AuditReader.GetCurrentRevision<AuditRevisionEntity>(false).Id;
        }

        /// <inheritdoc />
        public long GetMaxRevision()
        {
            return this.AuditReader.GetMaxRevision();
        }

        public abstract IEnumerable<ObjectModification> GetModifiedObjectsFromLogic();

        public abstract IEnumerable<ObjectModification> GetModifiedObjectsFromLogic<TPersistentDomainObjectBase>();

        public abstract void ManualFault();

        public IObjectStateService GetObjectStateService()
        {
            return new NHibObjectStatesService(this.InnerSession);
        }

        public IDALFactory<TPersistentDomainObjectBase, TIdent> GetDALFactory<TPersistentDomainObjectBase, TIdent>()
            where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        {
            return new NHibDalFactory<TPersistentDomainObjectBase, TIdent>(this);
        }

        protected virtual void OnClosed([NotNull] EventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            this.Closed?.Invoke(this, e);
        }

        protected virtual void OnFlushed([NotNull] DALChangesEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            this.Flushed?.Invoke(this, e);

            this.SessionFactory.OnSessionFlushed(new SessionFlushedEventArgs(e.Changes, this));
        }

        protected virtual void OnBeforeTransactionCompleted([NotNull] DALChangesEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            this.BeforeTransactionCompleted?.Invoke(this, e);

            this.SessionFactory.OnSessionBeforeTransactionCompleted(new SessionFlushedEventArgs(e.Changes, this));
        }

        protected virtual void OnAfterTransactionCompleted([NotNull] DALChangesEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            this.AfterTransactionCompleted?.Invoke(this, e);

            this.SessionFactory.OnSessionAfterTransactionCompleted(new SessionFlushedEventArgs(e.Changes, this));
        }

        protected void ClearClosed()
        {
            this.Closed = null;
        }

        protected void ClearFlushed()
        {
            this.Flushed = null;
        }

        protected void ClearTransactionCompleted()
        {
            this.BeforeTransactionCompleted = null;
            this.AfterTransactionCompleted = null;
        }

        public event EventHandler Closed;

        public event EventHandler<DALChangesEventArgs> Flushed;

        [Obsolete("Since 6.0 Use AfterTransactionCompleted", true)]
        public event EventHandler<DALChangesEventArgs> TransactionCompleted;

        public event EventHandler<DALChangesEventArgs> BeforeTransactionCompleted;

        public event EventHandler<DALChangesEventArgs> AfterTransactionCompleted;

        public abstract void Dispose();
    }
}

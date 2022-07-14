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
    public abstract class NHibSessionBase : IDBSession
    {
        private Lazy<IAuditReaderPatched> lazyAuditReader { get; }

        internal NHibSessionBase(NHibSessionConfiguration sessionFactory, DBSessionMode sessionMode)
        {
            this.SessionConfiguration = sessionFactory ?? throw new ArgumentNullException(nameof(sessionFactory));
            this.SessionMode = sessionMode;

            this.lazyAuditReader = LazyHelper.Create(() => this.InnerSession.GetAuditReader());


            this.InnerSession = this.SessionConfiguration.InternalSessionFactory.OpenSession();
            this.InnerSession.FlushMode = FlushMode.Manual;
        }

        public DBSessionMode SessionMode { get; }

        public IAuditReaderPatched AuditReader => this.lazyAuditReader.Value;

        public ISession InnerSession { get; }

        protected internal NHibSessionConfiguration SessionConfiguration { get; }

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

        /// <inheritdoc />
        public abstract void AsReadOnly();

        /// <inheritdoc />
        public abstract void AsWritable();

        public IObjectStateService GetObjectStateService()
        {
            return new NHibObjectStatesService(this.InnerSession);
        }

        public IDALFactory<TPersistentDomainObjectBase, TIdent> GetDALFactory<TPersistentDomainObjectBase, TIdent>()
            where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        {
            return new NHibDalFactory<TPersistentDomainObjectBase, TIdent>(this);
        }

        protected virtual void OnFlushed([NotNull] DALChangesEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            this.Flushed?.Invoke(this, e);
        }

        protected virtual void OnBeforeTransactionCompleted([NotNull] DALChangesEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            this.BeforeTransactionCompleted?.Invoke(this, e);
        }

        protected virtual void OnAfterTransactionCompleted([NotNull] DALChangesEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            this.AfterTransactionCompleted?.Invoke(this, e);
        }
        protected virtual void OnClosed([NotNull] EventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            this.Closed?.Invoke(this, e);
        }

        protected void ClearEvents()
        {
            this.Flushed = null;
            this.BeforeTransactionCompleted = null;
            this.AfterTransactionCompleted = null;
            this.Closed = null;
        }

        public event EventHandler<DALChangesEventArgs> Flushed;

        public event EventHandler<DALChangesEventArgs> BeforeTransactionCompleted;

        public event EventHandler<DALChangesEventArgs> AfterTransactionCompleted;

        public event EventHandler Closed;

        public abstract void Dispose();
    }



    //public class NHibSession
    //{
    //    public NHibSession()
    //    {

    //    }

    //    private IDBSession Create(DBSessionMode openMode)
    //    {
    //        if (DBSessionMode.Read == openMode)
    //        {
    //            return new ReadOnlyNHibSession(this);
    //        }

    //        return new WriteNHibSession(this);
    //    }

    //    protected internal void OnSessionFlushed([NotNull] SessionFlushedEventArgs e)
    //    {
    //        if (e == null) throw new ArgumentNullException(nameof(e));

    //        this.SessionFlushed.Maybe(handler => handler(this, e));
    //    }

    //    protected internal virtual void OnSessionAfterTransactionCompleted([NotNull] SessionFlushedEventArgs e)
    //    {
    //        if (e == null) { throw new ArgumentNullException(nameof(e)); }

    //        this.SessionAfterTransactionCompleted?.Invoke(this, e);
    //    }

    //    protected internal virtual void OnSessionBeforeTransactionCompleted([NotNull] SessionFlushedEventArgs e)
    //    {
    //        if (e == null) { throw new ArgumentNullException(nameof(e)); }

    //        this.SessionBeforeTransactionCompleted?.Invoke(this, e);
    //    }


    //    /// <summary> Session flushed event
    //    /// </summary>
    //    public event EventHandler<SessionFlushedEventArgs> SessionFlushed;

    //    /// <summary> Transaction completed event
    //    /// </summary>
    //    [Obsolete("Use SessionAfterTransactionCompleted", true)]
    //    public event EventHandler<SessionFlushedEventArgs> SessionTransactionCompleted;

    //    public event EventHandler<SessionFlushedEventArgs> SessionBeforeTransactionCompleted;

    //    public event EventHandler<SessionFlushedEventArgs> SessionAfterTransactionCompleted;
    //}
}

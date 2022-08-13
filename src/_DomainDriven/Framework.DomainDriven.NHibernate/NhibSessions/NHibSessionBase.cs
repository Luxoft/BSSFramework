using System;
using System.Collections.Generic;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Tracking;
using Framework.DomainDriven.DAL.Revisions;
using Framework.Persistent;

using NHibernate;
using NHibernate.Envers.Patch;

namespace Framework.DomainDriven.NHibernate
{
    public abstract class NHibSessionBase : IDBSession
    {
        private Lazy<IAuditReaderPatched> lazyAuditReader { get; }

        internal NHibSessionBase(NHibSessionEnvironment environment, DBSessionMode sessionMode)
        {
            this.Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.SessionMode = sessionMode;

            this.lazyAuditReader = LazyHelper.Create(() => this.InnerSession.GetAuditReader());
        }

        public abstract bool Closed { get; }

        public DBSessionMode SessionMode { get; }

        public IAuditReaderPatched AuditReader => this.lazyAuditReader.Value;

        public abstract ISession InnerSession { get; }

        protected internal NHibSessionEnvironment Environment { get; }

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

        public abstract void AsFault();

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

        public abstract void Close();

        public void Dispose()
        {
            if (!this.Closed)
            {

            }

            this.Close();
        }
    }
}

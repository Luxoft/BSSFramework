using System;
using System.Collections.Generic;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DAL.Revisions;

namespace Framework.DomainDriven.NHibernate
{
    internal class ReadOnlyNHibSession : NHibSession
    {
        private bool disposed;

        internal ReadOnlyNHibSession(NHibSessionFactory sessionFactory)
                : base(sessionFactory, DBSessionMode.Read)
        {
            this.InnerSession.DefaultReadOnly = true;
        }

        public override DBSessionMode Mode { get; } = DBSessionMode.Read;

        public override IEnumerable<ObjectModification> GetModifiedObjectsFromLogic()
        {
            yield break;
        }

        public override IEnumerable<ObjectModification> GetModifiedObjectsFromLogic<TPersistentDomainObjectBase>()
        {
            yield break;
        }

        public override void ManualFault()
        {

        }

        public override void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;

            try
            {
                this.OnClosed(EventArgs.Empty);
            }
            finally
            {
                this.ClearClosed();
            }
        }

        public override void Flush()
        {
            throw new InvalidOperationException();
        }

        public override void RegisterModified<T>(T @object, ModificationType modificationType)
        {
            throw new ArgumentException($"Not supported: '{nameof(this.RegisterModified)}' for {this.GetType()}");
        }
    }
}

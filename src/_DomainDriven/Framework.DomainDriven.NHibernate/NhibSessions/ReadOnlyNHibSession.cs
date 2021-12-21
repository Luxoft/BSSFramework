using System;
using System.Collections.Generic;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DAL.Revisions;
using NHibernate;

namespace Framework.DomainDriven.NHibernate
{
    internal class ReadOnlyNHibSession : NHibSession
    {
        internal ReadOnlyNHibSession(NHibSessionFactory sessionFactory) : base(sessionFactory, DBSessionMode.Read)
        {
            if (sessionFactory == null) throw new ArgumentNullException(nameof(sessionFactory));

            this.SessionFactory = sessionFactory;
        }

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

        public override TResult Evaluate<TResult>(Func<IDBSession, TResult> getResult)
        {
            using (this.InnerSession = this.SessionFactory.InternalSessionFactory.OpenSession())
            {
                this.InnerSession.FlushMode = FlushMode.Never;
                this.InnerSession.DefaultReadOnly = true;

                try
                {
                    var result = getResult(this);

                    this.OnClosed(EventArgs.Empty);

                    return result;
                }
                finally
                {
                    this.ClearClosed();
                }
            }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override void RegisterModifited<T>(T @object, ModificationType modificationType)
        {
            throw new ArgumentException($"Not supported: '{nameof(this.RegisterModifited)}' for {this.GetType()}");
        }
    }
}
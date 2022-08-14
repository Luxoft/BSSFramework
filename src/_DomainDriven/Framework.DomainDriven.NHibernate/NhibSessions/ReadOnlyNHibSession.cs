using System;
using System.Collections.Generic;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DAL.Revisions;

using NHibernate;

namespace Framework.DomainDriven.NHibernate
{
    public class ReadOnlyNHibSession : NHibSessionBase
    {
        private bool closed;

        public ReadOnlyNHibSession(NHibSessionEnvironment environment)
                : base(environment, DBSessionMode.Read)
        {
            this.InnerSession = this.Environment.InternalSessionFactory.OpenSession();
            this.InnerSession.FlushMode = FlushMode.Manual;
            this.InnerSession.DefaultReadOnly = true;
        }

        public override bool Closed => this.closed;

        public sealed override ISession InnerSession { get; }

        public override IEnumerable<ObjectModification> GetModifiedObjectsFromLogic()
        {
            yield break;
        }

        public override IEnumerable<ObjectModification> GetModifiedObjectsFromLogic<TPersistentDomainObjectBase>()
        {
            yield break;
        }

        public override void AsFault()
        {
        }

        public override void AsReadOnly()
        {
        }

        public override void AsWritable()
        {
            throw new InvalidOperationException("Readonly session already created");
        }

        public override void Close()
        {
            if (this.closed)
            {
                return;
            }

            this.closed = true;

            using (this.InnerSession) ;
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

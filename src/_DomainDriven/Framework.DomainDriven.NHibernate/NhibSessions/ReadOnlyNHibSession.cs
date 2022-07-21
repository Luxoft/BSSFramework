using System;
using System.Collections.Generic;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DAL.Revisions;

namespace Framework.DomainDriven.NHibernate
{
    internal class ReadOnlyNHibSession : NHibSessionBase
    {
        private bool disposed;

        internal ReadOnlyNHibSession(NHibSessionEnvironment environment)
                : base(environment, DBSessionMode.Read)
        {
            this.InnerSession.DefaultReadOnly = true;
        }

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
            if (this.InnerSession.IsOpen)
            {
                this.InnerSession.Close();
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

﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
            this.NativeSession = this.Environment.InternalSessionFactory.OpenSession();
            this.NativeSession.FlushMode = FlushMode.Manual;
            this.NativeSession.DefaultReadOnly = true;
        }

        public override bool Closed => this.closed;

        public sealed override ISession NativeSession { get; }


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

        public override async Task CloseAsync(CancellationToken cancellationToken = default)
        {
            if (this.closed)
            {
                return;
            }

            this.closed = true;


            using (this.NativeSession);
        }

        public override Task FlushAsync(CancellationToken cancellationToken = default)
        {
            throw new InvalidOperationException();
        }

        public override void RegisterModified<T>(T @object, ModificationType modificationType)
        {
            throw new ArgumentException($"Not supported: '{nameof(this.RegisterModified)}' for {this.GetType()}");
        }
    }
}

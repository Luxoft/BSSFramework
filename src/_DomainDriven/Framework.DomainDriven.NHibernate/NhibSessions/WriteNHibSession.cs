using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;

using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven.Audit;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DAL.Revisions;
using Framework.DomainDriven.NHibernate.Audit;

using JetBrains.Annotations;

using NHibernate;
using NHibernate.Event;
using NHibernate.Impl;

using IsolationLevel = System.Transactions.IsolationLevel;

namespace Framework.DomainDriven.NHibernate
{
    public class WriteNHibSession : NHibSessionBase
    {
        [NotNull]
        private readonly IEnumerable<IAuditProperty> modifyAuditProperties;

        [NotNull]
        private readonly IEnumerable<IAuditProperty> createAuditProperties;

        private readonly ISet<ObjectModification> modifiedObjectsFromLogic = new HashSet<ObjectModification>();

        private readonly CollectChangesEventListener collectChangedEventListener;

        private readonly TransactionScope transactionScope;

        private readonly ITransaction transaction;

        private bool manualFault;

        private bool disposed;

        internal WriteNHibSession(NHibSessionConfiguration sessionFactory,
                                  [NotNull] IEnumerable<IAuditProperty> modifyAuditProperties,
                                  [NotNull] IEnumerable<IAuditProperty> createAuditProperties)
                : base(sessionFactory, DBSessionMode.Write)
        {
            this.modifyAuditProperties = modifyAuditProperties;
            this.createAuditProperties = createAuditProperties;
            this.collectChangedEventListener = new CollectChangesEventListener();

            this.transactionScope = this.SessionConfiguration.EnableTransactionScope ? this.CreateTransactionScope() : null;

            this.transaction = this.InnerSession.BeginTransaction();

            this.SessionConfiguration.ProcessTransaction(GetDbTransaction(this.transaction, this.InnerSession));

            this.ConfigureEventListeners();
        }


        private void ConfigureEventListeners()
        {
            var sessionImplementation = this.InnerSession.GetSessionImplementation();

            var sessionImpl = (SessionImpl)sessionImplementation;

            sessionImpl.OverrideListeners(this.CreateEventListeners());

            sessionImpl.OverrideInterceptor(this.CreateInterceptor());
        }

        private EventListeners CreateEventListeners()
        {
            var result = new EventListeners();

            result.PostDeleteEventListeners = new[] { this.collectChangedEventListener };
            result.PostUpdateEventListeners = new[] { this.collectChangedEventListener };
            result.PostInsertEventListeners = new[] { this.collectChangedEventListener };

            if (this.SessionConfiguration.ConnectionSettings.UseEventListenerInsteadOfInterceptorForAudit)
            {
                var modifyAuditEventListener = new ModifyAuditEventListener(this.modifyAuditProperties);
                var createAuditEventListener = new CreateAuditEventListener(this.createAuditProperties);
#pragma warning restore 0618

                result.PreUpdateEventListeners = new IPreUpdateEventListener[] { modifyAuditEventListener };
                result.PreInsertEventListeners = new IPreInsertEventListener[] { modifyAuditEventListener, createAuditEventListener };

#pragma warning disable 0618 // Obsolete
            }


            return result;
        }

        private IInterceptor CreateInterceptor()
        {
            if (this.SessionConfiguration.ConnectionSettings.UseEventListenerInsteadOfInterceptorForAudit)
            {
                return new EmptyInterceptor();
            }
            else
            {
                return new AuditInterceptor(this.createAuditProperties, this.modifyAuditProperties);
            }
        }

        public override IEnumerable<ObjectModification> GetModifiedObjectsFromLogic()
        {
            return this.modifiedObjectsFromLogic;
        }

        public override IEnumerable<ObjectModification> GetModifiedObjectsFromLogic<TPersistentDomainObjectBase>()
        {
            return this.GetModifiedObjectsFromLogic()
                       .Where(obj => typeof(TPersistentDomainObjectBase).IsAssignableFrom(obj.ObjectType));
        }

        public override void ManualFault()
        {
            this.manualFault = true;
        }

        public override void AsReadOnly()
        {
            throw new InvalidOperationException("Writable session already created");
        }

        public override void AsWritable()
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
                using (this.transactionScope)
                using (this.transaction)
                {
                    if (this.manualFault)
                    {
                        if (!this.transaction.WasRolledBack)
                        {
                            this.transaction.Rollback();
                        }
                    }
                    else
                    {
                        this.Flush(true);

                        this.transaction.Commit();

                        this.OnClosed(EventArgs.Empty);

                        this.transactionScope?.Complete();
                    }
                }
            }
            finally
            {
                this.ClearEvents();
            }
        }

        private static IDbTransaction GetDbTransaction(ITransaction transaction, ISession session)
        {
            // https://stackoverflow.com/questions/40231650/can-i-get-the-underlying-conneciton-and-transaction-objects-from-nhibernate
            using var dbCommand = session.Connection.CreateCommand();
            dbCommand.Cancel();
            transaction.Enlist(dbCommand);

            return dbCommand.Transaction;
        }

        private TransactionScope CreateTransactionScope() =>
                new(
                    TransactionScopeOption.Required,
                    new TransactionOptions
                    {
                            Timeout = this.SessionConfiguration.TransactionTimeout,
                            IsolationLevel = IsolationLevel.Serializable
                    });

        /// <inheritdoc />
        public override void Flush()
        {
            this.Flush(false);
        }

        private void Flush(bool withTransactionCompleted)
        {
            try
            {
                var dalHistory = new List<DALChanges>();

                do
                {
                    this.InnerSession.Flush();

                    var changes = this.collectChangedEventListener.EvictChanges();

                    if (changes.IsEmpty)
                    {
                        break;
                    }
                    else
                    {
                        dalHistory.Add(changes);

                        this.AuditReader.GetCurrentRevision(true);

                        var changedEventArgs = new DALChangesEventArgs(changes);

                        this.OnFlushed(changedEventArgs);
                    }
                } while (this.HasFlushedListeners);

                if (withTransactionCompleted)
                {
                    var beforeTransactionCompletedChangeState = dalHistory.Composite();

                    this.OnBeforeTransactionCompleted(new DALChangesEventArgs(beforeTransactionCompletedChangeState));

                    this.InnerSession.Flush();

                    var afterTransactionCompletedChangeState =
                            new[] { beforeTransactionCompletedChangeState, this.collectChangedEventListener.EvictChanges() }
                                    .Composite();

                    this.OnAfterTransactionCompleted(new DALChangesEventArgs(afterTransactionCompletedChangeState));

                    this.InnerSession
                        .Flush(); // Флашим для того, чтобы проверить, что никто ничего не менял в объектах после AfterTransactionCompleted-евента

                    if (this.collectChangedEventListener.HasAny())
                    {
                        throw new InvalidOperationException("DomainObjects can't be changed after TransactionCompleted event");
                    }
                }
            }
            catch (Exception e)
            {
                var result = this.SessionConfiguration.ExceptionProcessor.Process(e);

                if (result == e)
                {
                    throw;
                }
                else
                {
                    throw result;
                }
            }
        }

        public override void RegisterModified<T>(T @object, ModificationType modificationType)
        {
            this.modifiedObjectsFromLogic.Add(new ObjectModification(@object, typeof(T), modificationType));
        }
    }
}

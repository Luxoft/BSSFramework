using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.DAL.Revisions;

using NHibernate;
using NHibernate.Event;
using NHibernate.Impl;

using IsolationLevel = System.Transactions.IsolationLevel;

namespace Framework.DomainDriven.NHibernate
{
    public class WriteNHibSession : NHibSession
    {
        private readonly ISet<ObjectModification> modifiedObjectsFromLogic = new HashSet<ObjectModification>();

        private readonly CollectChangesEventListener collectChangedEventListener;

        private readonly TransactionScope transactionScope;

        private readonly ITransaction currentTransaction;

        private bool manualFault;

        private bool disposed;

        internal WriteNHibSession(NHibSessionFactory sessionFactory)
                : base(sessionFactory, DBSessionMode.Write)
        {
            this.collectChangedEventListener = new CollectChangesEventListener();

            this.transactionScope = this.SessionFactory.EnableTransactionScope ? this.CreateTransactionScope() : null;

            this.currentTransaction = this.InnerSession.BeginTransaction();

            this.SessionFactory.ProcessTransaction(GetDbTransaction(this.currentTransaction, this.InnerSession));

            this.InnerSessionConfigure();
        }
        public override DBSessionMode Mode { get; } = DBSessionMode.Write;

        private EventListeners GetOverrideEventListenersFrom(EventListeners source)
        {
            var result = new EventListeners();

            result.PostDeleteEventListeners =
                    source.PostDeleteEventListeners.Concat(new[] { this.collectChangedEventListener }).ToArray();
            result.PostUpdateEventListeners =
                    source.PostUpdateEventListeners.Concat(new[] { this.collectChangedEventListener }).ToArray();
            result.PostInsertEventListeners =
                    source.PostInsertEventListeners.Concat(new[] { this.collectChangedEventListener }).ToArray();

            result.LoadEventListeners = source.LoadEventListeners;
            result.SaveOrUpdateEventListeners = source.SaveOrUpdateEventListeners;
            result.MergeEventListeners = source.MergeEventListeners;
            result.PersistEventListeners = source.PersistEventListeners;
            result.PersistOnFlushEventListeners = source.PersistOnFlushEventListeners;
            result.ReplicateEventListeners = source.ReplicateEventListeners;
            result.DeleteEventListeners = source.DeleteEventListeners;
            result.AutoFlushEventListeners = source.AutoFlushEventListeners;
            result.DirtyCheckEventListeners = source.DirtyCheckEventListeners;
            result.FlushEventListeners = source.FlushEventListeners;
            result.EvictEventListeners = source.EvictEventListeners;
            result.LockEventListeners = source.LockEventListeners;
            result.RefreshEventListeners = source.RefreshEventListeners;
            result.FlushEntityEventListeners = source.FlushEntityEventListeners;
            result.InitializeCollectionEventListeners = source.InitializeCollectionEventListeners;
            result.PostLoadEventListeners = source.PostLoadEventListeners;
            result.PreLoadEventListeners = source.PreLoadEventListeners;
            result.PreDeleteEventListeners = source.PreDeleteEventListeners;
            result.PreUpdateEventListeners = source.PreUpdateEventListeners;
            result.PreInsertEventListeners = source.PreInsertEventListeners;
            result.PostCommitDeleteEventListeners = source.PostCommitDeleteEventListeners;
            result.PostCommitUpdateEventListeners = source.PostCommitUpdateEventListeners;
            result.PostCommitInsertEventListeners = source.PostCommitInsertEventListeners;
            result.PreCollectionRecreateEventListeners = source.PreCollectionRecreateEventListeners;
            result.PostCollectionRecreateEventListeners = source.PostCollectionRecreateEventListeners;
            result.PreCollectionRemoveEventListeners = source.PreCollectionRemoveEventListeners;
            result.PostCollectionRemoveEventListeners = source.PostCollectionRemoveEventListeners;
            result.PreCollectionUpdateEventListeners = source.PreCollectionUpdateEventListeners;
            result.PostCollectionUpdateEventListeners = source.PostCollectionUpdateEventListeners;
            result.SaveEventListeners = source.SaveEventListeners;
            result.UpdateEventListeners = source.UpdateEventListeners;

            return result;
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

        public override void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;


            try
            {
                using var scope = this.transactionScope;
                using var transaction = this.currentTransaction;

                if (this.manualFault)
                {
                    if (!this.currentTransaction.WasRolledBack)
                    {
                        this.currentTransaction.Rollback();
                    }
                }
                else
                {
                    this.Flush(true);

                    this.currentTransaction.Commit();

                    this.OnClosed(EventArgs.Empty);

                    scope?.Complete();
                }
            }
            finally
            {
                this.ClearClosed();
                this.ClearFlushed();
                this.ClearTransactionCompleted();
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

        private void InnerSessionConfigure()
        {
            var sessionImplementation = this.InnerSession.GetSessionImplementation();

            var sessionImpl = (SessionImpl)sessionImplementation;

            sessionImpl.OverrideListeners(this.GetOverrideEventListenersFrom(sessionImplementation.Listeners));
        }

        private TransactionScope CreateTransactionScope() =>
                new(
                    TransactionScopeOption.Required,
                    new TransactionOptions
                    {
                            Timeout = this.SessionFactory.TransactionTimeout,
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
                var result = this.SessionFactory.ExceptionProcessor.Process(e);

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

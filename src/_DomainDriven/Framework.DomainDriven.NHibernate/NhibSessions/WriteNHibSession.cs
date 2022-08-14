using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

using Framework.Core;
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
        private readonly IDBSessionEventListener[] eventListeners;

        [NotNull]
        private readonly AuditPropertyPair modifyAuditProperties;

        [NotNull]
        private readonly AuditPropertyPair createAuditProperties;

        private readonly ISet<ObjectModification> modifiedObjectsFromLogic = new HashSet<ObjectModification>();

        private readonly CollectChangesEventListener collectChangedEventListener;

        private readonly TransactionScope transactionScope;

        private readonly ITransaction transaction;

        private bool manualFault;

        private bool closed;

        public WriteNHibSession(NHibSessionEnvironment environment,
                                INHibSessionSetup settings,
                                IEnumerable<IDBSessionEventListener> eventListeners)
                : base(environment, DBSessionMode.Write)
        {
            this.eventListeners = eventListeners.ToArray();
            this.modifyAuditProperties = settings.GetModifyAuditProperty();
            this.createAuditProperties = settings.GetCreateAuditProperty();
            this.collectChangedEventListener = new CollectChangesEventListener();

            this.transactionScope = this.Environment.EnableTransactionScope ? this.CreateTransactionScope() : null;

            this.InnerSession = this.Environment.InternalSessionFactory.OpenSession();
            this.InnerSession.FlushMode = FlushMode.Manual;

            this.transaction = this.InnerSession.BeginTransaction();

            this.Environment.ProcessTransaction(GetDbTransaction(this.transaction, this.InnerSession));

            this.ConfigureEventListeners();
        }

        public override bool Closed => this.closed;

        public sealed override ISession InnerSession { get; }

        private void ConfigureEventListeners()
        {
            var sessionImplementation = this.InnerSession.GetSessionImplementation();

            var sessionImpl = (SessionImpl)sessionImplementation;

            sessionImpl.OverrideListeners(sessionImpl.Listeners.Clone().Self(this.InjectListeners));

            sessionImpl.OverrideInterceptor(this.CreateInterceptor());
        }

        private void InjectListeners(EventListeners eventListeners)
        {
            eventListeners.PostDeleteEventListeners = eventListeners.PostDeleteEventListeners.Concat(new[] { this.collectChangedEventListener }).ToArray();
            eventListeners.PostUpdateEventListeners = eventListeners.PostUpdateEventListeners.Concat(new[] { this.collectChangedEventListener }).ToArray();
            eventListeners.PostInsertEventListeners = eventListeners.PostInsertEventListeners.Concat(new[] { this.collectChangedEventListener }).ToArray();

            if (this.Environment.ConnectionSettings.UseEventListenerInsteadOfInterceptorForAudit)
            {
                var modifyAuditEventListener = new ModifyAuditEventListener(this.modifyAuditProperties);
                var createAuditEventListener = new CreateAuditEventListener(this.createAuditProperties);
#pragma warning restore 0618

                eventListeners.PreUpdateEventListeners = eventListeners.PreUpdateEventListeners.Concat(new IPreUpdateEventListener[] { modifyAuditEventListener }).ToArray();
                eventListeners.PreInsertEventListeners = eventListeners.PreInsertEventListeners.Concat(new IPreInsertEventListener[] { modifyAuditEventListener, createAuditEventListener }).ToArray();

#pragma warning disable 0618 // Obsolete
            }
        }

        private IInterceptor CreateInterceptor()
        {
            if (this.Environment.ConnectionSettings.UseEventListenerInsteadOfInterceptorForAudit)
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

        public override void AsFault()
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

        public override async Task CloseAsync()
        {
            if (this.closed)
            {
                return;
            }

            this.closed = true;

            using (this.transactionScope)
            {
                using (this.InnerSession)
                {
                    using (this.transaction)
                    {
                        if (this.manualFault)
                        {
                            if (!this.transaction.WasRolledBack)
                            {
                                await this.transaction.RollbackAsync();
                            }
                        }
                        else
                        {
                            await this.FlushAsync(true);

                            await this.transaction.CommitAsync();
                            this.transactionScope?.Complete();
                        }
                    }
                }
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
                            Timeout = this.Environment.TransactionTimeout,
                            IsolationLevel = IsolationLevel.Serializable
                    });

        public override async Task FlushAsync()
        {
            await this.FlushAsync(false);
        }

        private async Task FlushAsync(bool withCompleteTransaction)
        {
            try
            {
                var dalHistory = new List<DALChanges>();

                do
                {
                    await this.InnerSession.FlushAsync();

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

                        // WARNING: You can't invoke the listeners if ServiceProvider is in dispose state!!! Use UseTryCloseDbSession middleware
                        this.eventListeners.Foreach(eventListener => eventListener.OnFlushed(changedEventArgs));
                    }
                } while (true);

                if (withCompleteTransaction)
                {
                    var beforeTransactionCompletedChangeState = dalHistory.Composite();

                    // WARNING: You can't invoke the listeners if ServiceProvider is in dispose state!!!!!! Use UseTryCloseDbSession middleware
                    this.eventListeners.Foreach(eventListener => eventListener.OnBeforeTransactionCompleted(new DALChangesEventArgs(beforeTransactionCompletedChangeState)));

                    await this.InnerSession.FlushAsync();

                    var afterTransactionCompletedChangeState =
                            new[] { beforeTransactionCompletedChangeState, this.collectChangedEventListener.EvictChanges() }
                                    .Composite();

                    // WARNING: You can't invoke the listeners if ServiceProvider is in dispose state!!!!!! Use UseTryCloseDbSession middleware
                    this.eventListeners.Foreach(eventListener => eventListener.OnAfterTransactionCompleted(new DALChangesEventArgs(afterTransactionCompletedChangeState)));

                    await this.InnerSession.FlushAsync(); // Флашим для того, чтобы проверить, что никто ничего не менял в объектах после AfterTransactionCompleted-евента

                    if (this.collectChangedEventListener.HasAny())
                    {
                        throw new InvalidOperationException("DomainObjects can't be changed after TransactionCompleted event");
                    }
                }
            }
            catch (Exception e)
            {
                var result = this.Environment.ExceptionProcessor.Process(e);

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

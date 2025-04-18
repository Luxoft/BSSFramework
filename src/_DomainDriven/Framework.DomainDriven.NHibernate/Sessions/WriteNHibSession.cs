﻿using System.Data;

using Framework.Core;
using Framework.DomainDriven.Audit;
using Framework.DomainDriven.NHibernate.Audit;

using NHibernate;
using NHibernate.Envers.Patch;
using NHibernate.Event;
using NHibernate.Impl;

namespace Framework.DomainDriven.NHibernate;

public class WriteNHibSession : NHibSessionBase
{
    private readonly IDBSessionEventListener[] eventListeners;


    private readonly AuditPropertyPair modifyAuditProperties;


    private readonly AuditPropertyPair createAuditProperties;

    private readonly CollectChangesEventListener collectChangedEventListener;

    private readonly ITransaction nhibTransaction;

    private bool manualFault;

    private bool closed;

    public WriteNHibSession(NHibSessionEnvironment environment,
                            IDBSessionSettings settings,
                            IEnumerable<IDBSessionEventListener> eventListeners)
            : base(environment, DBSessionMode.Write)
    {
        this.eventListeners = eventListeners.ToArray();
        this.modifyAuditProperties = settings.GetModifyAuditProperty();
        this.createAuditProperties = settings.GetCreateAuditProperty();
        this.collectChangedEventListener = new CollectChangesEventListener();

        this.NativeSession = this.Environment.InternalSessionFactory.OpenSession();
        this.NativeSession.FlushMode = FlushMode.Manual;

        this.nhibTransaction = this.NativeSession.BeginTransaction();

        this.Transaction = GetDbTransaction(this.nhibTransaction, this.NativeSession);

        this.ConfigureEventListeners();
    }

    public override bool Closed => this.closed;

    public sealed override ISession NativeSession { get; }

    public override IDbTransaction Transaction { get; }

    private void ConfigureEventListeners()
    {
        var sessionImplementation = this.NativeSession.GetSessionImplementation();

        var sessionImpl = (SessionImpl)sessionImplementation;

        sessionImpl.OverrideListeners(sessionImpl.Listeners.Clone().Self(this.InjectListeners));

        sessionImpl.OverrideInterceptor(new AuditInterceptor(this.createAuditProperties, this.modifyAuditProperties));
    }

    private void InjectListeners(EventListeners eventListeners)
    {
        eventListeners.PostDeleteEventListeners = eventListeners.PostDeleteEventListeners.Concat(new[] { this.collectChangedEventListener }).ToArray();
        eventListeners.PostUpdateEventListeners = eventListeners.PostUpdateEventListeners.Concat(new[] { this.collectChangedEventListener }).ToArray();
        eventListeners.PostInsertEventListeners = eventListeners.PostInsertEventListeners.Concat(new[] { this.collectChangedEventListener }).ToArray();
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

    public override async Task CloseAsync(CancellationToken cancellationToken = default)
    {
        if (this.closed)
        {
            return;
        }

        this.closed = true;

        using (this.NativeSession)
        {
            using (this.nhibTransaction)
            {
                if (this.manualFault)
                {
                    if (!this.nhibTransaction.WasRolledBack)
                    {
                        await this.nhibTransaction.RollbackAsync(cancellationToken);
                    }
                }
                else
                {
                    await this.FlushAsync(true, cancellationToken);

                    await this.nhibTransaction.CommitAsync(cancellationToken);
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

    public override async Task FlushAsync(CancellationToken cancellationToken = default)
    {
        await this.FlushAsync(false, cancellationToken);
    }

    private async Task FlushAsync(bool withCompleteTransaction, CancellationToken cancellationToken)
    {
        try
        {
            var dalHistory = new List<DALChanges>();

            do
            {
                await this.NativeSession.FlushAsync(cancellationToken);

                var changes = this.collectChangedEventListener.EvictChanges();

                if (changes.IsEmpty)
                {
                    break;
                }
                else
                {
                    dalHistory.Add(changes);

                    await this.AuditReader.SafeInitCurrentRevisionAsync(cancellationToken);

                    var changedEventArgs = new DALChangesEventArgs(changes);

                    // WARNING: You can't invoke the listeners if ServiceProvider is in dispose state!!! Use UseTryCloseDbSession middleware
                    this.eventListeners.Foreach(eventListener =>
                                                {
                                                    cancellationToken.ThrowIfCancellationRequested();

                                                    eventListener.OnFlushed(changedEventArgs);
                                                });
                }
            } while (true);

            if (withCompleteTransaction)
            {
                var beforeTransactionCompletedChangeState = dalHistory.Composite();

                // WARNING: You can't invoke the listeners if ServiceProvider is in dispose state!!!!!! Use UseTryCloseDbSession middleware
                this.eventListeners.Foreach(eventListener =>
                                            {
                                                cancellationToken.ThrowIfCancellationRequested();

                                                eventListener.OnBeforeTransactionCompleted(new DALChangesEventArgs(beforeTransactionCompletedChangeState));
                                            });

                await this.NativeSession.FlushAsync(cancellationToken);

                var afterTransactionCompletedChangeState =
                        new[] { beforeTransactionCompletedChangeState, this.collectChangedEventListener.EvictChanges() }
                                .Composite();

                // WARNING: You can't invoke the listeners if ServiceProvider is in dispose state!!!!!! Use UseTryCloseDbSession middleware
                this.eventListeners.Foreach(eventListener => eventListener.OnAfterTransactionCompleted(new DALChangesEventArgs(afterTransactionCompletedChangeState)));

                await this.NativeSession.FlushAsync(cancellationToken); // Флашим для того, чтобы проверить, что никто ничего не менял в объектах после AfterTransactionCompleted-евента

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
}

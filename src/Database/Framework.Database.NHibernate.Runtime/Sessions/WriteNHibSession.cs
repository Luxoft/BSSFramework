using System.Data;

using Framework.Core;
using Framework.Database.NHibernate.EnversAudit;
using Framework.Database.NHibernate.InlineAudit;
using NHibernate;
using NHibernate.Event;
using NHibernate.Impl;

namespace Framework.Database.NHibernate.Sessions;

/// <summary>
/// Writable NHibernate session with its own transaction (opened in the constructor).
/// Uses <see cref="FlushMode.Manual"/>: nothing is written until <see cref="FlushAsync(CancellationToken)"/> or <see cref="CloseAsync"/>,
/// so reads inside the session may see unflushed state only through the first-level cache.
/// The commit-time flow (listeners, final "nothing changed" check) mirrors <c>WriteEfSession</c> in the EntityFramework provider.
/// </summary>
public class WriteNHibSession : NHibSessionBase
{
    private readonly NHibSessionEnvironment environment;

    private readonly IInlineAuditInterceptor inlineAuditInterceptor;

    private readonly IDBSessionEventListener[] eventListeners;

    private readonly CollectChangesEventListener collectChangedEventListener;

    private readonly ITransaction nhibTransaction;

    private bool manualFault;

    private bool closed;

    public WriteNHibSession(
        NHibSessionEnvironment environment,
        IInlineAuditInterceptor inlineAuditInterceptor,
        IEnumerable<IDBSessionEventListener> eventListeners)
    {
        this.environment = environment;
        this.inlineAuditInterceptor = inlineAuditInterceptor;
        this.eventListeners = eventListeners.ToArray();
        this.collectChangedEventListener = new CollectChangesEventListener();

        this.NativeSession = this.environment.InternalSessionFactory.OpenSession();
        this.NativeSession.FlushMode = FlushMode.Manual;

        this.nhibTransaction = this.NativeSession.BeginTransaction();

        this.Transaction = GetDbTransaction(this.nhibTransaction, this.NativeSession);

        this.ConfigureEventListeners();
    }

    public override bool Closed => this.closed;

    public override DBSessionMode SessionMode { get; } = DBSessionMode.Write;

    public sealed override ISession NativeSession { get; }

    public override IDbTransaction Transaction { get; }

    /// <summary>
    /// Customizes listeners and interceptor of this particular session instance (not of the shared SessionFactory).
    /// <c>OverrideListeners</c>/<c>OverrideInterceptor</c> are internal-ish APIs of <see cref="SessionImpl"/>, hence the cast:
    /// the session must be a <see cref="SessionImpl"/>, other ISession implementations (e.g. wrappers) are not supported.
    /// </summary>
    private void ConfigureEventListeners()
    {
        var sessionImplementation = this.NativeSession.GetSessionImplementation();

        var sessionImpl = (SessionImpl)sessionImplementation;

        sessionImpl.OverrideListeners(sessionImpl.Listeners.Clone().Self(this.InjectListeners));

        sessionImpl.OverrideInterceptor(this.inlineAuditInterceptor);
    }

    /// <summary>
    /// Appends the change collector to the post-insert/update/delete listeners.
    /// Concat is used on purpose: the default listeners must keep running, they are copied by <c>Listeners.Clone()</c>.
    /// Using the global configuration instead would also change listeners of every other session
    /// (see the SessionConfiguration remark in the NHibernate setup).
    /// </summary>
    private void InjectListeners(EventListeners newSessionEventListeners)
    {
        newSessionEventListeners.PostDeleteEventListeners = newSessionEventListeners.PostDeleteEventListeners.Concat([this.collectChangedEventListener]).ToArray();
        newSessionEventListeners.PostUpdateEventListeners = newSessionEventListeners.PostUpdateEventListeners.Concat([this.collectChangedEventListener]).ToArray();
        newSessionEventListeners.PostInsertEventListeners = newSessionEventListeners.PostInsertEventListeners.Concat([this.collectChangedEventListener]).ToArray();
    }

    public override void AsFault() => this.manualFault = true;

    public override void AsReadOnly() => throw new InvalidOperationException("Writable session already created");

    public override void AsWritable()
    {
    }

    public override async Task CloseAsync(CancellationToken ct)
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
                        await this.nhibTransaction.RollbackAsync(ct);
                    }
                }
                else
                {
                    await this.FlushAsync(true, ct);

                    await this.nhibTransaction.CommitAsync(ct);
                }
            }
        }
    }


    /// <summary>
    /// NHibernate doesn't expose the underlying <see cref="IDbTransaction"/>, so it is obtained indirectly:
    /// <c>ITransaction.Enlist</c> assigns the current transaction to a throw-away command, which is never executed
    /// and is disposed right away; the transaction is then read back from the command.
    /// </summary>
    private static IDbTransaction GetDbTransaction(ITransaction transaction, ISession session)
    {
        // https://stackoverflow.com/questions/40231650/can-i-get-the-underlying-conneciton-and-transaction-objects-from-nhibernate
        using var dbCommand = session.Connection.CreateCommand();
        dbCommand.Cancel();
        transaction.Enlist(dbCommand);

        return dbCommand.Transaction!;
    }

    public override async Task FlushAsync(CancellationToken ct) => await this.FlushAsync(false, ct);

    /// <summary>
    /// Flushes until stable: listeners (<c>OnFlushed</c>) may change entities, so flush is repeated while the collector reports changes.
    /// With <paramref name="withCompleteTransaction"/> additionally runs OnBeforeTransactionCompleted (may still change data)
    /// and OnAfterTransactionCompleted (must not change data - the last flush is a check, it throws if anything is dirty).
    /// </summary>
    private async Task FlushAsync(bool withCompleteTransaction, CancellationToken ct)
    {
        try
        {
            var dalHistory = new List<DALChanges>();

            do
            {
                await this.NativeSession.FlushAsync(ct);

                var changes = this.collectChangedEventListener.EvictChanges();

                if (changes.IsEmpty)
                {
                    break;
                }
                else
                {
                    dalHistory.Add(changes);

                    await this.AuditReader.SafeInitCurrentRevisionAsync(ct);

                    var changedEventArgs = new DALChangesEventArgs(changes);

                    // WARNING: You can't invoke the listeners if ServiceProvider is in dispose state!!! Use UseTryCloseDbSession middleware
                    foreach (var eventListener in this.eventListeners)
                    {
                        ct.ThrowIfCancellationRequested();

                        await eventListener.OnFlushed(changedEventArgs, ct);
                    }
                }
            } while (true);

            if (withCompleteTransaction)
            {
                var beforeTransactionCompletedChangeState = dalHistory.Composite();

                // WARNING: You can't invoke the listeners if ServiceProvider is in dispose state!!!!!! Use UseTryCloseDbSession middleware
                foreach (var eventListener in this.eventListeners)
                {
                    ct.ThrowIfCancellationRequested();

                    await eventListener.OnBeforeTransactionCompleted(new DALChangesEventArgs(beforeTransactionCompletedChangeState), ct);
                }

                await this.NativeSession.FlushAsync(ct);

                var afterTransactionCompletedChangeState =
                    new[] { beforeTransactionCompletedChangeState, this.collectChangedEventListener.EvictChanges() }
                        .Composite();

                // WARNING: You can't invoke the listeners if ServiceProvider is in dispose state!!!!!! Use UseTryCloseDbSession middleware
                foreach (var eventListener in this.eventListeners)
                {
                    ct.ThrowIfCancellationRequested();

                    await eventListener.OnAfterTransactionCompleted(new DALChangesEventArgs(afterTransactionCompletedChangeState), ct);
                }

                await this.NativeSession.FlushAsync(
                    ct); // Flush to verify that nobody changed anything in the objects after the AfterTransactionCompleted event

                if (this.collectChangedEventListener.HasAny())
                {
                    throw new InvalidOperationException("DomainObjects can't be changed after TransactionCompleted event");
                }
            }
        }
        catch (Exception ex)
        {
            var expandedException = this.environment.InternalExceptionExpander.Expand(ex);

            if (expandedException == ex)
            {
                throw;
            }
            else
            {
                throw expandedException;
            }
        }
    }
}

using System.Data;

using Framework.Core;
using Framework.Database.AuditProperty;
using Framework.Database.NHibernate.Audit;
using Framework.Database.NHibernate.Envers;

using NHibernate;
using NHibernate.Event;
using NHibernate.Impl;

namespace Framework.Database.NHibernate.Sessions;

public class WriteNHibSession : IDBSession<ISession>
{
    private readonly NHibSessionEnvironment environment;

    private readonly IAuditReaderPatched auditReader;

    private readonly IDBSessionEventListener[] eventListeners;


    private readonly AuditPropertyPair modifyAuditProperties;


    private readonly AuditPropertyPair createAuditProperties;

    private readonly CollectChangesEventListener collectChangedEventListener;

    private readonly ITransaction nhibTransaction;

    private bool manualFault;

    public WriteNHibSession(
        NHibSessionEnvironment environment,
        IAuditPropertyFactory auditPropertyFactory,
        IAuditReaderPatched auditReader,
        IEnumerable<IDBSessionEventListener> eventListeners)
    {
        this.environment = environment;
        this.auditReader = auditReader;
        this.eventListeners = eventListeners.ToArray();
        this.modifyAuditProperties = auditPropertyFactory.GetModifyAuditProperty();
        this.createAuditProperties = auditPropertyFactory.GetCreateAuditProperty();
        this.collectChangedEventListener = new CollectChangesEventListener();

        this.NativeSession = environment.InternalSessionFactory.OpenSession();
        this.NativeSession.FlushMode = FlushMode.Manual;

        this.nhibTransaction = this.NativeSession.BeginTransaction();

        this.Transaction = GetDbTransaction(this.nhibTransaction, this.NativeSession);

        this.ConfigureEventListeners();
    }

    public DBSessionMode SessionMode { get; } = DBSessionMode.Write;

    public ISession NativeSession { get; }

    public bool Closed { get; private set; }

    public IDbTransaction Transaction { get; }

    private void ConfigureEventListeners()
    {
        var sessionImplementation = this.NativeSession.GetSessionImplementation();

        var sessionImpl = (SessionImpl)sessionImplementation;

        sessionImpl.OverrideListeners(sessionImpl.Listeners.Clone().Self(this.InjectListeners));

        sessionImpl.OverrideInterceptor(new AuditInterceptor(this.createAuditProperties, this.modifyAuditProperties));
    }

    private void InjectListeners(EventListeners newSessionEventListeners)
    {
        newSessionEventListeners.PostDeleteEventListeners =
            newSessionEventListeners.PostDeleteEventListeners.Concat([this.collectChangedEventListener]).ToArray();
        newSessionEventListeners.PostUpdateEventListeners =
            newSessionEventListeners.PostUpdateEventListeners.Concat([this.collectChangedEventListener]).ToArray();
        newSessionEventListeners.PostInsertEventListeners =
            newSessionEventListeners.PostInsertEventListeners.Concat([this.collectChangedEventListener]).ToArray();
    }

    public void AsFault() => this.manualFault = true;

    public void AsReadOnly() => throw new InvalidOperationException("Writable session already created");

    public void AsWritable()
    {
    }

    public async Task CloseAsync(CancellationToken ct)
    {
        if (this.Closed)
        {
            return;
        }

        this.Closed = true;

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


    private static IDbTransaction GetDbTransaction(ITransaction transaction, ISession session)
    {
        // https://stackoverflow.com/questions/40231650/can-i-get-the-underlying-conneciton-and-transaction-objects-from-nhibernate
        using var dbCommand = session.Connection.CreateCommand();
        dbCommand.Cancel();
        transaction.Enlist(dbCommand);

        return dbCommand.Transaction!;
    }

    public async Task FlushAsync(CancellationToken ct) => await this.FlushAsync(false, ct);

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

                    await this.auditReader.SafeInitCurrentRevisionAsync(ct);

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
                    ct); // Флашим для того, чтобы проверить, что никто ничего не менял в объектах после AfterTransactionCompleted-евента

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

    public async ValueTask DisposeAsync() => await this.CloseAsync(CancellationToken.None);
}

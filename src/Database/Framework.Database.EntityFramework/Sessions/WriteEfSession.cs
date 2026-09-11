using System.Data;

using Framework.Core;
using Framework.Database.EntityFramework.SqlExceptionProcessors;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Framework.Database.EntityFramework.Sessions;

public class WriteEfSession : IEfSession
{
    private readonly IDBSessionEventListener[] eventListeners;

    private readonly IExceptionExpander exceptionExpander;

    private readonly EfCollectChangesService collectChangesService = new();

    private readonly RelationalTransaction efTransaction;

    private bool manualFault;

    public WriteEfSession(DbContext nativeSession, IEnumerable<IDBSessionEventListener> eventListeners, IEfSqlExceptionExpander exceptionExpander)
    {
        this.NativeSession = nativeSession;
        this.eventListeners = eventListeners.ToArray();
        this.exceptionExpander = exceptionExpander;

        this.efTransaction = (RelationalTransaction)nativeSession.Database.BeginTransaction();
        this.Transaction = this.efTransaction.GetDbTransaction();
    }

    public DBSessionMode SessionMode { get; } = DBSessionMode.Write;

    public DbContext NativeSession { get; }

    public bool Closed { get; private set; }

    public IDbTransaction Transaction { get; }


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

        await using (this.NativeSession)
        {
            await using (this.efTransaction)
            {
                if (this.manualFault)
                {
                    if (this.Transaction.Connection is not null)
                    {
                        await this.efTransaction.RollbackAsync(ct);
                    }
                }
                else
                {
                    await this.FlushAsync(true, ct);

                    await this.efTransaction.CommitAsync(ct);
                }
            }
        }
    }

    public async Task FlushAsync(CancellationToken ct) => await this.FlushAsync(false, ct);

    private void IncrementConcurrencyVersionTokens()
    {
        foreach (var entry in this.NativeSession.ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:

                    // NHibernate seeds a versioned property to 1 on insert (IVersionType.Seed); mirror that here so a
                    // freshly inserted entity's Version doesn't stay at its default(long) value of 0 across providers.
                    foreach (var property in entry.Properties)
                    {
                        if (property.Metadata.IsConcurrencyToken && property.Metadata.ClrType == typeof(long) && Equals(property.CurrentValue, 0L))
                        {
                            property.CurrentValue = 1L;
                        }
                    }

                    break;

                case EntityState.Modified:

                    foreach (var property in entry.Properties)
                    {
                        if (property.Metadata.IsConcurrencyToken && property.Metadata.ClrType == typeof(long) && !property.IsModified)
                        {
                            property.CurrentValue = (long)property.CurrentValue! + 1;
                        }
                    }

                    break;
            }
        }
    }

    private async Task FlushAsync(bool withCompleteTransaction, CancellationToken ct)
    {
        try
        {
            var dalHistory = new List<DALChanges>();

            do
            {
                var changes = this.collectChangesService.CollectChanges(this.NativeSession);

                this.IncrementConcurrencyVersionTokens();

                await this.NativeSession.SaveChangesAsync(ct);

                if (changes.IsEmpty)
                {
                    break;
                }
                else
                {
                    dalHistory.Add(changes);

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

                var listenersChanges = this.collectChangesService.CollectChanges(this.NativeSession);

                this.IncrementConcurrencyVersionTokens();

                await this.NativeSession.SaveChangesAsync(ct);

                var afterTransactionCompletedChangeState =
                    new[] { beforeTransactionCompletedChangeState, listenersChanges }
                        .Composite();

                // WARNING: You can't invoke the listeners if ServiceProvider is in dispose state!!!!!! Use UseTryCloseDbSession middleware
                foreach (var eventListener in this.eventListeners)
                {
                    ct.ThrowIfCancellationRequested();

                    await eventListener.OnAfterTransactionCompleted(new DALChangesEventArgs(afterTransactionCompletedChangeState), ct);
                }

                var finalChanges = this.collectChangesService.CollectChanges(this.NativeSession);

                await this.NativeSession
                          .SaveChangesAsync(
                              ct); // Флашим для того, чтобы проверить, что никто ничего не менял в объектах после AfterTransactionCompleted-евента

                if (!finalChanges.IsEmpty)
                {
                    throw new InvalidOperationException("DomainObjects can't be changed after TransactionCompleted event");
                }
            }
        }
        catch (Exception ex)
        {
            var expandedException = this.exceptionExpander.Expand(ex);

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

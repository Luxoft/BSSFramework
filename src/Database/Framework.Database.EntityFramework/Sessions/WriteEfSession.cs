using System.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Framework.Database.EntityFramework.Sessions;

public class WriteEfSession : EfSessionBase
{
    private readonly IDBSessionEventListener[] eventListeners;

    private readonly RelationalTransaction efTransaction;

    private bool manualFault;

    private bool closed;

    public WriteEfSession(DbContext nativeSession,
                          IEnumerable<IDBSessionEventListener> eventListeners)
            : base(nativeSession, DBSessionMode.Write)
    {
        this.eventListeners = eventListeners.ToArray();

        this.efTransaction = (RelationalTransaction)this.NativeSession.Database.BeginTransaction();
        this.Transaction = this.efTransaction.GetDbTransaction();
    }

    public override bool Closed => this.closed;

    public override IDbTransaction Transaction { get; }


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
            using (this.efTransaction)
            {
                if (this.manualFault)
                {
                    if (this.Transaction.Connection != null)
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

    public override async Task FlushAsync(CancellationToken ct) => await this.FlushAsync(false, ct);

    private async Task FlushAsync(bool withCompleteTransaction, CancellationToken ct)
    {
        try
        {
            var dalHistory = new List<DALChanges>();

            do
            {
                await this.NativeSession.SaveChangesAsync(ct);

                break;
                //var changes = this.collectChangedEventListener.EvictChanges();

                //if (changes.IsEmpty)
                //{
                //    break;
                //}
                //else
                //{
                //    dalHistory.Add(changes);

                //    await this.AuditReader.SafeInitCurrentRevisionAsync(ct);

                //    var changedEventArgs = new DALChangesEventArgs(changes);

                //    // WARNING: You can't invoke the listeners if ServiceProvider is in dispose state!!! Use UseTryCloseDbSession middleware
                //    this.eventListeners.Foreach(eventListener =>
                //                                {
                //                                    ct.ThrowIfCancellationRequested();

                //                                    eventListener.OnFlushed(changedEventArgs);
                //                                });
                //}
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

                await this.NativeSession.SaveChangesAsync(ct);

                //var afterTransactionCompletedChangeState =
                //        new[] { beforeTransactionCompletedChangeState, this.collectChangedEventListener.EvictChanges() }
                //                .Composite();

                // WARNING: You can't invoke the listeners if ServiceProvider is in dispose state!!!!!! Use UseTryCloseDbSession middleware
                //this.eventListeners.Foreach(eventListener => eventListener.OnAfterTransactionCompleted(new DALChangesEventArgs(afterTransactionCompletedChangeState)));

                await this.NativeSession.SaveChangesAsync(ct); // Флашим для того, чтобы проверить, что никто ничего не менял в объектах после AfterTransactionCompleted-евента

                //if (this.collectChangedEventListener.HasAny())
                //{
                //    throw new InvalidOperationException("DomainObjects can't be changed after TransactionCompleted event");
                //}
            }
        }
        catch (Exception e)
        {
            throw;


            //var result = this.Environment.ExceptionProcessor.Process(e);

            //if (result == e)
            //{
            //    throw;
            //}
            //else
            //{
            //    throw result;
            //}
        }
    }
}


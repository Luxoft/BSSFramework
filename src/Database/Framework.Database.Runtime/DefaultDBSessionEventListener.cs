using Framework.Database.DALListener;

namespace Framework.Database;

public class DefaultDBSessionEventListener(
    IInitializeManager initializeManager,
    IEnumerable<IFlushedDALListener> flushedDALListener,
    IEnumerable<IBeforeTransactionCompletedDALListener> beforeTransactionCompletedDALListener,
    IEnumerable<IAfterTransactionCompletedDALListener> afterTransactionCompletedDALListener)
    : IDBSessionEventListener
{
    private readonly IReadOnlyCollection<IFlushedDALListener> flushedDALListener = flushedDALListener.ToArray();

    private readonly IReadOnlyCollection<IBeforeTransactionCompletedDALListener> beforeTransactionCompletedDALListener = beforeTransactionCompletedDALListener.ToArray();

    private readonly IReadOnlyCollection<IAfterTransactionCompletedDALListener> afterTransactionCompletedDALListener = afterTransactionCompletedDALListener.ToArray();

    public async Task OnFlushed(DALChangesEventArgs eventArgs, CancellationToken cancellationToken)
    {
        if (initializeManager.IsInitialize)
        {
            return;
        }

        foreach (var listener in this.flushedDALListener)
        {
            await listener.Process(eventArgs, cancellationToken);
        }
    }

    public async Task OnBeforeTransactionCompleted(DALChangesEventArgs eventArgs, CancellationToken cancellationToken)
    {
        if (initializeManager.IsInitialize)
        {
            return;
        }

        foreach (var listener in this.beforeTransactionCompletedDALListener)
        {
            await listener.Process(eventArgs, cancellationToken);
        }
    }

    public async Task OnAfterTransactionCompleted(DALChangesEventArgs eventArgs, CancellationToken cancellationToken)
    {
        if (initializeManager.IsInitialize)
        {
            return;
        }

        foreach (var listener in this.afterTransactionCompletedDALListener)
        {
            await listener.Process(eventArgs, cancellationToken);
        }
    }
}

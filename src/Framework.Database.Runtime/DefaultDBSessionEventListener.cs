using Framework.Database.DALListener;

namespace Framework.Database;

public class DefaultDbSessionEventListener(
    IInitializeManager initializeManager,
    IEnumerable<IFlushedDalListener> flushedDalListener,
    IEnumerable<IBeforeTransactionCompletedDalListener> beforeTransactionCompletedDalListener,
    IEnumerable<IAfterTransactionCompletedDalListener> afterTransactionCompletedDalListener)
    : IDBSessionEventListener
{
    private readonly IReadOnlyCollection<IFlushedDalListener> flushedDalListener = flushedDalListener.ToArray();

    private readonly IReadOnlyCollection<IBeforeTransactionCompletedDalListener> beforeTransactionCompletedDalListener = beforeTransactionCompletedDalListener.ToArray();

    private readonly IReadOnlyCollection<IAfterTransactionCompletedDalListener> afterTransactionCompletedDalListener = afterTransactionCompletedDalListener.ToArray();

    public async Task OnFlushed(DALChangesEventArgs eventArgs, CancellationToken cancellationToken)
    {
        if (initializeManager.IsInitialize)
        {
            return;
        }

        foreach (var listener in this.flushedDalListener)
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

        foreach (var listener in this.beforeTransactionCompletedDalListener)
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

        foreach (var listener in this.afterTransactionCompletedDalListener)
        {
            await listener.Process(eventArgs, cancellationToken);
        }
    }
}

namespace Framework.DomainDriven;

public class DefaultDBSessionEventListener(
    IInitializeManager initializeManager,
    IEnumerable<IFlushedDALListener> flushedDalListener,
    IEnumerable<IBeforeTransactionCompletedDALListener> beforeTransactionCompletedDalListener,
    IEnumerable<IAfterTransactionCompletedDALListener> afterTransactionCompletedDalListener)
    : IDBSessionEventListener
{
    private readonly IReadOnlyCollection<IFlushedDALListener> flushedDalListener = flushedDalListener.ToArray();

    private readonly IReadOnlyCollection<IBeforeTransactionCompletedDALListener> beforeTransactionCompletedDalListener = beforeTransactionCompletedDalListener.ToArray();

    private readonly IReadOnlyCollection<IAfterTransactionCompletedDALListener> afterTransactionCompletedDalListener = afterTransactionCompletedDalListener.ToArray();

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
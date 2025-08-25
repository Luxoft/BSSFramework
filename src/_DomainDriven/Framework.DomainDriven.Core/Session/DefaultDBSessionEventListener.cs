using CommonFramework;

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

    public void OnFlushed(DALChangesEventArgs eventArgs)
    {
        if (initializeManager.IsInitialize)
        {
            return;
        }

        this.flushedDalListener.Foreach(listener => listener.Process(eventArgs));
    }

    public void OnBeforeTransactionCompleted(DALChangesEventArgs eventArgs)
    {
        if (initializeManager.IsInitialize)
        {
            return;
        }

        this.beforeTransactionCompletedDalListener.Foreach(listener => listener.Process(eventArgs));
    }

    public void OnAfterTransactionCompleted(DALChangesEventArgs eventArgs)
    {
        if (initializeManager.IsInitialize)
        {
            return;
        }

        this.afterTransactionCompletedDalListener.Foreach(listener => listener.Process(eventArgs));
    }
}

using Framework.Core;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class DefaultDBSessionEventListener : IDBSessionEventListener
{
    private readonly IInitializeManager initializeManager;

    private readonly IReadOnlyCollection<IFlushedDALListener> flushedDalListener;

    private readonly IReadOnlyCollection<IBeforeTransactionCompletedDALListener> beforeTransactionCompletedDalListener;

    private readonly IReadOnlyCollection<IAfterTransactionCompletedDALListener> afterTransactionCompletedDalListener;


    public DefaultDBSessionEventListener(
            IInitializeManager initializeManager,
            IEnumerable<IFlushedDALListener> flushedDalListener,
            IEnumerable<IBeforeTransactionCompletedDALListener> beforeTransactionCompletedDalListener,
            IEnumerable<IAfterTransactionCompletedDALListener> afterTransactionCompletedDalListener)
    {
        this.initializeManager = initializeManager;

        this.flushedDalListener = flushedDalListener.ToArray();
        this.beforeTransactionCompletedDalListener = beforeTransactionCompletedDalListener.ToArray();
        this.afterTransactionCompletedDalListener = afterTransactionCompletedDalListener.ToArray();
    }

    public void OnFlushed(DALChangesEventArgs eventArgs)
    {
        if (this.initializeManager.IsInitialize)
        {
            return;
        }

        this.flushedDalListener.Foreach(listener => listener.Process(eventArgs));
    }

    public void OnBeforeTransactionCompleted(DALChangesEventArgs eventArgs)
    {
        if (this.initializeManager.IsInitialize)
        {
            return;
        }

        this.beforeTransactionCompletedDalListener.Foreach(listener => listener.Process(eventArgs));
    }

    public void OnAfterTransactionCompleted(DALChangesEventArgs eventArgs)
    {
        if (this.initializeManager.IsInitialize)
        {
            return;
        }

        this.afterTransactionCompletedDalListener.Foreach(listener => listener.Process(eventArgs));
    }
}

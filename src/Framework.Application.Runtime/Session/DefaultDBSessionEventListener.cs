using Framework.Application.DALListener;
using Framework.Application.Session.DALChanges;

namespace Framework.Application.Session;

public class DefaultDbSessionEventListener(
    IInitializeManager initializeManager,
    IEnumerable<IFlushedDalListener> flushedDalListener,
    IEnumerable<IBeforeTransactionCompletedDalListener> beforeTransactionCompletedDalListener,
    IEnumerable<IAfterTransactionCompletedDalListener> afterTransactionCompletedDalListener)
    : IdbSessionEventListener
{
    private readonly IReadOnlyCollection<IFlushedDalListener> flushedDalListener = flushedDalListener.ToArray();

    private readonly IReadOnlyCollection<IBeforeTransactionCompletedDalListener> beforeTransactionCompletedDalListener = beforeTransactionCompletedDalListener.ToArray();

    private readonly IReadOnlyCollection<IAfterTransactionCompletedDalListener> afterTransactionCompletedDalListener = afterTransactionCompletedDalListener.ToArray();

    public async Task OnFlushed(DalChangesEventArgs eventArgs, CancellationToken cancellationToken)
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

    public async Task OnBeforeTransactionCompleted(DalChangesEventArgs eventArgs, CancellationToken cancellationToken)
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

    public async Task OnAfterTransactionCompleted(DalChangesEventArgs eventArgs, CancellationToken cancellationToken)
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
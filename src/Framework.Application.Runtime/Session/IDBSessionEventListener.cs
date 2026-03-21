using Framework.Application.Session.DALChanges;

namespace Framework.Application.Session;

public interface IDBSessionEventListener
{
    Task OnFlushed(DALChangesEventArgs eventArgs, CancellationToken cancellationToken);

    Task OnBeforeTransactionCompleted(DALChangesEventArgs eventArgs, CancellationToken cancellationToken);

    Task OnAfterTransactionCompleted(DALChangesEventArgs eventArgs, CancellationToken cancellationToken);
}

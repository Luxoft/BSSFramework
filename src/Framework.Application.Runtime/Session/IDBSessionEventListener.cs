using Framework.Application.Session.DALChanges;

namespace Framework.Application.Session;

public interface IdbSessionEventListener
{
    Task OnFlushed(DalChangesEventArgs eventArgs, CancellationToken cancellationToken);

    Task OnBeforeTransactionCompleted(DalChangesEventArgs eventArgs, CancellationToken cancellationToken);

    Task OnAfterTransactionCompleted(DalChangesEventArgs eventArgs, CancellationToken cancellationToken);
}

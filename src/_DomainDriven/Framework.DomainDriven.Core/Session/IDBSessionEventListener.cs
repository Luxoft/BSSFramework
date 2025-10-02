namespace Framework.DomainDriven;

public interface IDBSessionEventListener
{
    Task OnFlushed(DALChangesEventArgs eventArgs, CancellationToken cancellationToken);

    Task OnBeforeTransactionCompleted(DALChangesEventArgs eventArgs, CancellationToken cancellationToken);

    Task OnAfterTransactionCompleted(DALChangesEventArgs eventArgs, CancellationToken cancellationToken);
}

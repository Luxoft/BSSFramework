namespace Framework.Database;

public interface IDBSessionEventListener
{
    Task OnFlushed(DALChangesEventArgs eventArgs, CancellationToken ct);

    Task OnBeforeTransactionCompleted(DALChangesEventArgs eventArgs, CancellationToken ct);

    Task OnAfterTransactionCompleted(DALChangesEventArgs eventArgs, CancellationToken ct);
}

namespace Framework.DomainDriven;

public interface IDBSessionEventListener
{
    void OnFlushed(DALChangesEventArgs eventArgs);

    void OnBeforeTransactionCompleted(DALChangesEventArgs eventArgs);

    void OnAfterTransactionCompleted(DALChangesEventArgs eventArgs);
}

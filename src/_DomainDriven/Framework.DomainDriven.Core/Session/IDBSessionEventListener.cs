namespace Framework.DomainDriven.BLL;

public interface IDBSessionEventListener
{
    void OnFlushed(DALChangesEventArgs eventArgs);

    void OnBeforeTransactionCompleted(DALChangesEventArgs eventArgs);

    void OnAfterTransactionCompleted(DALChangesEventArgs eventArgs);
}

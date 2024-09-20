namespace Framework.DomainDriven;

public class SessionFlushedEventArgs(DALChanges changes, IDBSession session) : DALChangesEventArgs(changes)
{
    public IDBSession Session { get; } = session;
}

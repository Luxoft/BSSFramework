namespace Framework.Application.Session.DALChanges;

public class SessionFlushedEventArgs(DALChanges changes, IDBSession session) : DALChangesEventArgs(changes)
{
    public IDBSession Session { get; } = session;
}

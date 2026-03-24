namespace Framework.Application.Session.DALChanges;

public class SessionFlushedEventArgs(DalChanges changes, IDBSession session) : DalChangesEventArgs(changes)
{
    public IDBSession Session { get; } = session;
}

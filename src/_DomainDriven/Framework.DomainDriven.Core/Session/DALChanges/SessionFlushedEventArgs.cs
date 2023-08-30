namespace Framework.DomainDriven;

public class SessionFlushedEventArgs : DALChangesEventArgs
{
    public SessionFlushedEventArgs(DALChanges changes, IDBSession session)
            : base(changes)
    {
        if (session == null) throw new ArgumentNullException(nameof(session));

        this.Session = session;
    }


    public IDBSession Session { get; private set; }
}

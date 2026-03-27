using Framework.Application.Session;

// ReSharper disable once CheckNamespace
namespace Framework.Database;

public class SessionFlushedEventArgs(DALChanges changes, IDBSession session) : DALChangesEventArgs(changes)
{
    public IDBSession Session { get; } = session;
}

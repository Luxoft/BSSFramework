namespace Framework.Application.Session.DALChanges;

public class DALChangesEventArgs(DALChanges changes) : EventArgs
{
    public DALChanges Changes { get; } = changes;
}

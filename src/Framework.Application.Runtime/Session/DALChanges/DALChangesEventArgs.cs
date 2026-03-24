namespace Framework.Application.Session.DALChanges;

public class DalChangesEventArgs(DalChanges changes) : EventArgs
{
    public DalChanges Changes { get; } = changes;
}

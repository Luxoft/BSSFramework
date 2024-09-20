namespace Framework.DomainDriven;

public class DALChangesEventArgs(DALChanges changes) : EventArgs
{
    public DALChanges Changes { get; } = changes;
}

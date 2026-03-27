// ReSharper disable once CheckNamespace
namespace Framework.Database;

public class DALChangesEventArgs(DALChanges changes) : EventArgs
{
    public DALChanges Changes { get; } = changes;
}

namespace Framework.Database.DALListener;

/// <summary>
/// Consumer of DAL events
/// </summary>
public interface IDALListener
{
    Task Process(DALChangesEventArgs eventArgs, CancellationToken ct);
}

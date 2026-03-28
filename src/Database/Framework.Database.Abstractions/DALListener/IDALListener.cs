namespace Framework.Database.DALListener;

/// <summary>
/// Потребитель DAL-евентов
/// </summary>
public interface IDALListener
{
    Task Process(DALChangesEventArgs eventArgs, CancellationToken cancellationToken);
}

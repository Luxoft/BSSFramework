namespace Framework.Configuration.BLL;

public interface ITargetSystemInitializer
{
    Task Initialize(CancellationToken cancellationToken);
}

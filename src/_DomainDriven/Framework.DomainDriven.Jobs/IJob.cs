namespace Framework.DomainDriven.Jobs;

public interface IJob
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}

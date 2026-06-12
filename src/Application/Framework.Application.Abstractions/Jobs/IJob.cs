namespace Framework.Application.Jobs;

public interface IJob
{
    Task ExecuteAsync(CancellationToken ct);
}

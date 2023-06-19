using Serilog;

namespace Framework.HangfireCore.JobBase;

public abstract class JobBase : IJobBase
{
    protected JobBase(ILogger logger)
    {
        this.Logger = logger.ForContext(this.GetType());
    }

    protected ILogger Logger { get; }

    public async Task Execute()
    {
        try
        {
            this.Logger.Information("Job started");

            await this.ExecuteInternal(CancellationToken.None);

            this.Logger.Information("Job finished");
        }
        catch (Exception e)
        {
            this.Logger.Error(e, "Job failed");
            throw;
        }
    }

    protected abstract Task ExecuteInternal(CancellationToken cancellationToken);
}

using Framework.HangfireCore.JobServices;

using Serilog;

namespace Framework.HangfireCore.JobBase;

public abstract class SimpleJobBase : JobBase
{
    private readonly IScopedJobExecutor jobExecutor;

    protected SimpleJobBase(ILogger logger, IScopedJobExecutor jobExecutor)
        : base(logger)
    {
        this.jobExecutor = jobExecutor;
    }

    protected sealed override async Task ExecuteInternal(CancellationToken cancellationToken)
    {
        await this.jobExecutor.ExecuteAsync(async () =>
        {
            await this.ExecuteScoped(cancellationToken);
            return default(object);
        });
    }

    protected abstract Task ExecuteScoped(CancellationToken cancellationToken);
}

using Framework.HangfireCore.JobServices;

using MediatR;

using Serilog;

namespace Framework.HangfireCore.JobBase;

public abstract class SimpleMediatorJob<TCommand> : SimpleJobBase
    where TCommand : IRequest, new()
{
    private readonly IMediator mediator;

    protected SimpleMediatorJob(ILogger logger, IScopedJobExecutor jobExecutor, IMediator mediator)
        : base(logger, jobExecutor) =>
        this.mediator = mediator;

    protected sealed override async Task ExecuteScoped(CancellationToken cancellationToken)
        => await this.mediator.Send(new TCommand(), cancellationToken);
}

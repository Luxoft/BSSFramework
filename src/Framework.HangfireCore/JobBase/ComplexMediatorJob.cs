using MediatR;

using Microsoft.Extensions.Logging;

using Serilog;

namespace Framework.HangfireCore.JobBase;

public abstract class ComplexMediatorJob<TCommand> : JobBase
    where TCommand : IRequest, new()
{
    private readonly IMediator mediator;

    protected ComplexMediatorJob(ILogger logger, IMediator mediator)
        : base(logger) =>
        this.mediator = mediator;

    protected sealed override async Task ExecuteInternal(CancellationToken cancellationToken)
        => await this.mediator.Send(new TCommand(), cancellationToken);
}

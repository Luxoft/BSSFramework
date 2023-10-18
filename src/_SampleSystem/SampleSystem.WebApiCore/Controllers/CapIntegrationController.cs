using DotNetCore.CAP;

using Framework.DomainDriven;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.BLL;
using SampleSystem.BLL.Core.IntegrationEvens;

namespace SampleSystem.WebApiCore;

public class CapIntegrationController
{
    private readonly IMediator mediator;

    private readonly IServiceEvaluator<ISampleSystemBLLContext> contextEvaluator;

    public CapIntegrationController(
            IMediator mediator,
            IServiceEvaluator<ISampleSystemBLLContext> contextEvaluator)
    {
        this.mediator = mediator;
        this.contextEvaluator = contextEvaluator;
    }

    [CapSubscribe(nameof(TestIntegrationEvent))]
    [NonAction]
    public async Task TestIntegrationEvent(TestIntegrationEvent @event, CancellationToken token) =>
            await this.contextEvaluator.EvaluateAsync(DBSessionMode.Write, async _ => await this.mediator.Send(@event, token));
}

using DotNetCore.CAP;

using Framework.DomainDriven;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.BLL.Core.IntegrationEvens;

namespace SampleSystem.WebApiCore;

public class CapIntegrationController
{
    private readonly IServiceEvaluator<IMediator> mediatorEvaluator;

    public CapIntegrationController(IServiceEvaluator<IMediator> mediatorEvaluator)
    {
        this.mediatorEvaluator = mediatorEvaluator;
    }

    [CapSubscribe(nameof(TestIntegrationEvent))]
    [NonAction]
    public async Task TestIntegrationEvent(TestIntegrationEvent @event, CancellationToken token) =>
            await this.mediatorEvaluator.EvaluateAsync(DBSessionMode.Write, mediator => mediator.Send(@event, token));
}

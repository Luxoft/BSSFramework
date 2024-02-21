using DotNetCore.CAP;

using Framework.DomainDriven;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using SampleSystem.BLL._Command.CreateClassA.Intergation;
using SampleSystem.BLL.Core.IntegrationEvens;

namespace SampleSystem.WebApiCore;

public class CapIntegrationController : ICapSubscribe
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

    [CapSubscribe(nameof(ClassACreatedEvent))]
    [NonAction]
    public async Task ClassACreatedEvent(ClassACreatedEvent @event, CancellationToken token) =>
        await this.mediatorEvaluator.EvaluateAsync(DBSessionMode.Write, mediator => mediator.Send(@event, token));
}

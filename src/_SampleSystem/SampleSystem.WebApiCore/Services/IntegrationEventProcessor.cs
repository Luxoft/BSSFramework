using Bss.Platform.Events.Abstractions;
using Bss.Platform.Events.Interfaces;

using Framework.Database;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace SampleSystem.WebApiCore.Services;

public class IntegrationEventProcessor(IDBSessionEvaluator sessionEvaluator) : IIntegrationEventProcessor
{
    public Task ProcessAsync(IIntegrationEvent @event, CancellationToken ct) =>
        sessionEvaluator.EvaluateAsync(DBSessionMode.Write, x => x.GetRequiredService<IMediator>().Publish(@event, ct), ct);
}

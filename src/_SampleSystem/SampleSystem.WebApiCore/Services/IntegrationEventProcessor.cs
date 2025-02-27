using Bss.Platform.Events.Abstractions;
using Bss.Platform.Events.Interfaces;

using Framework.DomainDriven;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace SampleSystem.WebApiCore.Services;

public class IntegrationEventProcessor(IDBSessionEvaluator sessionEvaluator) : IIntegrationEventProcessor
{
    public Task ProcessAsync(IIntegrationEvent @event, CancellationToken cancellationToken) =>
        sessionEvaluator.EvaluateAsync<object>(
            DBSessionMode.Write,
            async x =>
            {
                await x.GetRequiredService<IMediator>().Publish(@event, cancellationToken);
                return default!;
            });
}

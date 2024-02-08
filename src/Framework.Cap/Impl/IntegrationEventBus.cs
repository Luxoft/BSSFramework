using System.Data;
using DotNetCore.CAP;

using Framework.Cap.Abstractions;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Cap.Impl;

public class IntegrationEventBus : IIntegrationEventBus
{
    private readonly ICapPublisher capPublisher;

    private readonly ICapTransaction capTransaction;

    public IntegrationEventBus(ICapPublisher capPublisher, ICapTransaction capTransaction)
    {
        this.capPublisher = capPublisher;
        this.capTransaction = capTransaction;
    }

    public async Task PublishAsync(IntegrationEvent @event, CancellationToken cancellationToken)
    {
        this.EnlistTransaction();

        await this.capPublisher.PublishAsync(@event.GetType().Name, @event, cancellationToken: cancellationToken);
    }

    public void Publish(IntegrationEvent @event)
    {
        this.EnlistTransaction();

        this.capPublisher.Publish(@event.GetType().Name, @event);
    }

    public void EnlistTransaction()
    {
        // Enlist current transaction to CAP

        this.capPublisher.Transaction.Value = this.capTransaction;
    }
}

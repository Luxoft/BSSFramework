using Microsoft.Extensions.Hosting;

namespace Framework.RabbitMq.Consumer.BackgroundServices;

public class RabbitMqBackgroundService : BackgroundService
{
    protected override Task ExecuteAsync(CancellationToken stoppingToken) => throw new NotImplementedException();
}

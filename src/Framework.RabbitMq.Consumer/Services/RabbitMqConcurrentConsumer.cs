using Framework.RabbitMq.Consumer.Interfaces;

using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Services;

public record RabbitMqConcurrentConsumer(
    IRabbitMqMessageReader MessageReader) : IRabbitMqConsumer
{
    public async Task ConsumeAsync(IModel channel, CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await this.MessageReader.ReadAsync(channel, token);
        }
    }

    public void Dispose()
    {
        // do nothing
    }
}

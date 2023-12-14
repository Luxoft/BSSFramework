using System.Text;

using Framework.RabbitMq.Consumer.Enums;
using Framework.RabbitMq.Consumer.Interfaces;
using Framework.RabbitMq.Consumer.Settings;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Services;

internal record MessageReader(
    IRabbitMqMessageProcessor MessageProcessor,
    ILogger<MessageReader> Logger,
    IOptions<RabbitMqConsumerSettings> ConsumerOptions) : IRabbitMqMessageReader
{
    private readonly RabbitMqConsumerSettings _settings = ConsumerOptions.Value;

    public async Task ReadAsync(IModel channel, CancellationToken token)
    {
        var result = channel.BasicGet(this._settings.Queue, false);
        if (result is null)
        {
            await Delay(this._settings.ReceiveMessageDelayMilliseconds, token);
            return;
        }

        await this.ProcessAsync(result, channel!, token);
    }

    private async Task ProcessAsync(BasicGetResult result, IModel channel, CancellationToken token)
    {
        try
        {
            var message = Encoding.UTF8.GetString(result.Body.Span);
            if (result.Redelivered && result.DeliveryTag > this._settings.FailedMessageRetryCount)
            {
                var behaviour = await this.MessageProcessor.ProcessDeadLetterAsync(
                                    result.BasicProperties,
                                    result.RoutingKey,
                                    message,
                                    token);
                if (behaviour == DeadLetterBehaviour.Skip)
                {
                    channel.BasicAck(result.DeliveryTag, false);
                    return;
                }
            }

            await this.MessageProcessor.ProcessAsync(result.BasicProperties, result.RoutingKey, message, token);
            channel.BasicAck(result.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            this.Logger.LogError(ex, "There was some problem with processing message. Routing key:'{RoutingKey}'", result.RoutingKey);
            channel.BasicNack(result.DeliveryTag, false, true);
            await Delay(this._settings.RejectMessageDelayMilliseconds, token);
        }
    }

    private static Task Delay(int value, CancellationToken token) => Task.Delay(TimeSpan.FromMilliseconds(value), token);
}

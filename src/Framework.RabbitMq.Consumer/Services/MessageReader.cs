using System.Text;

using Framework.RabbitMq.Consumer.Enums;
using Framework.RabbitMq.Consumer.Interfaces;
using Framework.RabbitMq.Consumer.Settings;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Polly;

using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Services;

internal record MessageReader(
    IRabbitMqMessageProcessor MessageProcessor,
    IDeadLetterProcessor DeadLetterProcessor,
    ILogger<MessageReader> Logger,
    IOptions<RabbitMqConsumerSettings> ConsumerOptions) : IRabbitMqMessageReader
{
    private const string RetryCountKey = "RetryCount";

    private readonly RabbitMqConsumerSettings _settings = ConsumerOptions.Value;

    public async Task ReadAsync(IModel channel, CancellationToken token)
    {
        var result = channel.BasicGet(this._settings.Queue, false);
        if (result is null)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(this._settings.ReceiveMessageDelayMilliseconds), token);
            return;
        }

        await this.ProcessAsync(result, channel, token);
    }

    private async Task ProcessAsync(BasicGetResult result, IModel channel, CancellationToken token) =>
        await Policy
              .HandleResult<bool>(x => !x)
              .WaitAndRetryForeverAsync(
                  (_, _) => TimeSpan.FromMilliseconds(this._settings.RejectMessageDelayMilliseconds),
                  (_, retryCount, _, ctx) =>
                  {
                      if (!ctx.Contains(RetryCountKey))
                          ctx.Add(RetryCountKey, retryCount);
                      else
                          ctx[RetryCountKey] = retryCount;
                  })
              .ExecuteAsync((ctx, innerToken) => this.ProcessAsync(result, channel, ctx, innerToken), new Context(), token);

    private async Task<bool> ProcessAsync(BasicGetResult result, IModel channel, Context retryContext, CancellationToken token)
    {
        try
        {
            var message = Encoding.UTF8.GetString(result.Body.Span);

            await this.MessageProcessor.ProcessAsync(result.BasicProperties, result.RoutingKey, message, token);
            channel.BasicAck(result.DeliveryTag, false);
            return true;
        }
        catch (Exception ex)
        {
            return await this.RetryAsync(result, channel, retryContext, ex);
        }
    }

    private async Task<bool> RetryAsync(BasicGetResult result, IModel channel, Context retryContext, Exception ex)
    {
        try
        {
            var retryCount = retryContext.Contains(RetryCountKey) ? (int?)retryContext[RetryCountKey] : null;
            if (retryCount == null || retryCount % this._settings.FailedMessageRetryCount != 0)
                return false;

            var decision = await this.DeadLetterProcessor.ProcessAsync(channel, result, ex);
            if (decision == DeadLetterDecision.Requeue) return false;

            channel.BasicAck(result.DeliveryTag, false);
            return true;
        }
        catch (Exception innerEx)
        {
            this.Logger.LogError(innerEx, "Failed to retry process message with routing key {RoutingKey}", result.RoutingKey);
            return false;
        }
    }
}

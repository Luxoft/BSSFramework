using Framework.RabbitMq.Consumer.Interfaces;
using Framework.RabbitMq.Consumer.Settings;

using Microsoft.Extensions.Options;

using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Services;

internal record ConsumerInitializer(IOptions<RabbitMqConsumerSettings> Options) : IRabbitMqConsumerInitializer
{
    public void Initialize(IModel model)
    {
        var consumerSettings = this.Options.Value;

        model.ExchangeDeclare(consumerSettings.Exchange, ExchangeType.Topic, true);
        model.QueueDeclare(consumerSettings.Queue, true, false, false, null);

        if (consumerSettings.RoutingKeys.Length == 0)
        {
            model.QueueBind(consumerSettings.Queue, consumerSettings.Exchange, "#");
            return;
        }

        foreach (var routingKey in consumerSettings.RoutingKeys)
            model.QueueBind(consumerSettings.Queue, consumerSettings.Exchange, routingKey);
    }
}

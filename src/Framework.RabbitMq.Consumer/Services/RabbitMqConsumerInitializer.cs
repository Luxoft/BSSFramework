using Framework.RabbitMq.Consumer.Interfaces;
using Framework.RabbitMq.Consumer.Settings;

using Microsoft.Extensions.Options;

using RabbitMQ.Client;

namespace Framework.RabbitMq.Consumer.Services;

public record RabbitMqConsumerInitializer(IOptions<RabbitMqSettings> Options) : IRabbitMqConsumerInitializer
{
    public void Initialize(IModel model)
    {
        var consumerSettings = this.Options.Value.Consumer;

        model.ExchangeDeclare(consumerSettings.Exchange, ExchangeType.Topic, true);
        model.QueueDeclare(consumerSettings.Queue, true, false, false, null);

        foreach (var routingKey in consumerSettings.RoutingKeys)
            model.QueueBind(consumerSettings.Queue, consumerSettings.Exchange, routingKey);
    }
}

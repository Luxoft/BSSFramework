using Framework.RabbitMq.Consumer.BackgroundServices;
using Framework.RabbitMq.Consumer.Interfaces;
using Framework.RabbitMq.Consumer.Services;
using Framework.RabbitMq.Consumer.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.RabbitMq.Consumer;

public static class DependencyInjection
{
    public static void AddRabbitMqConsumer<TMessageProcessor>(this IServiceCollection services, IConfiguration configuration)
        where TMessageProcessor : class, IRabbitMqMessageProcessor =>
        services
            .Configure<RabbitMqSettings>(configuration.GetSection("RabbitMQ"))
            .AddSingleton<IRabbitMqClient, RabbitMqClient>()
            .AddSingleton<IRabbitMqMessageProcessor, TMessageProcessor>()
            .AddSingleton<IRabbitMqConsumerInitializer, RabbitMqConsumerInitializer>()
            .AddHostedService<RabbitMqBackgroundService>();
}

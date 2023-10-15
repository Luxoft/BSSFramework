using Framework.RabbitMq.Consumer.BackgroundServices;
using Framework.RabbitMq.Consumer.Interfaces;
using Framework.RabbitMq.Consumer.Services;
using Framework.RabbitMq.Consumer.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.RabbitMq.Consumer;

public static class DependencyInjection
{
    public static IServiceCollection AddRabbitMqConsumer<TMessageProcessor>(this IServiceCollection services, IConfiguration configuration)
        where TMessageProcessor : class, IRabbitMqMessageProcessor =>
        services
            .Configure<RabbitMqConsumerSettings>(configuration.GetSection("RabbitMQ:Consumer"))
            .AddSingleton<IRabbitMqMessageProcessor, TMessageProcessor>()
            .AddSingleton<IRabbitMqConsumerInitializer, RabbitMqConsumerInitializer>()
            .AddHostedService<RabbitMqBackgroundService>();

    public static IServiceCollection AddRabbitMqConsumer<TMessageProcessor, TAuditService>(
        this IServiceCollection services,
        IConfiguration configuration)
        where TMessageProcessor : class, IRabbitMqMessageProcessor
        where TAuditService : class, IRabbitMqAuditService =>
        services
            .AddRabbitMqConsumer<TMessageProcessor>(configuration)
            .AddSingleton<IRabbitMqAuditService, TAuditService>();
}

using Framework.RabbitMq.Consumer.BackgroundServices;
using Framework.RabbitMq.Consumer.Enums;
using Framework.RabbitMq.Consumer.Interfaces;
using Framework.RabbitMq.Consumer.Services;
using Framework.RabbitMq.Consumer.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.RabbitMq.Consumer;

public static class DependencyInjection
{
    public static IServiceCollection AddRabbitMqConsumerFork<TMessageProcessor>(this IServiceCollection services, IConfiguration configuration)
        where TMessageProcessor : class, IRabbitMqMessageProcessor
    {
        var settings = new RabbitMqConsumerSettings();
        var settingsSection = configuration.GetSection("RabbitMQ:Consumer");
        settingsSection.Bind(settings);

        if (settings.Mode == RabbitMqConsumerMode.MultipleActiveConsumers)
        {
            services
                .AddSingleton<IRabbitMqConsumer, RabbitMqConcurrentConsumer>();
        }
        else
        {
            services
                .AddSingleton<IRabbitMqConsumer, RabbitMqSynchronizedConsumer>();
        }

        return services
            .Configure<RabbitMqConsumerSettings>(settingsSection)
            .AddSingleton<IRabbitMqMessageReader, RabbitMqMessageReader>()
            .AddSingleton<IRabbitMqMessageProcessor, TMessageProcessor>()
            .AddSingleton<IRabbitMqConsumerInitializer, RabbitMqConsumerInitializer>()
            .AddHostedService<RabbitMqBackgroundService>();
    }

    public static IServiceCollection AddRabbitMqSqlServerConsumerLock(this IServiceCollection services, string connectionString) =>
        services
            .AddSingleton<IRabbitMqConsumerLockService, RabbitMqSqlServerLockService>()
            .AddSingleton<IRabbitMqSqlSeverConnectionStringProvider>(new RabbitMqSqlSeverConnectionStringProvider(connectionString));
}

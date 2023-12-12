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
    public static IServiceCollection AddRabbitMqConsumer<TMessageProcessor>(this IServiceCollection services, IConfiguration configuration)
        where TMessageProcessor : class, IRabbitMqMessageProcessor
    {
        var settings = new RabbitMqConsumerSettings();
        var settingsSection = configuration.GetSection("RabbitMQ:Consumer");
        settingsSection.Bind(settings);

        if (settings.Mode == RabbitMqConsumerMode.Single)
        {
            services
                .AddSingleton<IRabbitMqConsumerSemaphore, RabbitMqSingleConsumerSemaphore>();
        }
        else
        {
            services
                .AddSingleton<IRabbitMqConsumerSemaphore, RabbitMqMultipleConsumersSemaphore>();
        }

        services
            .Configure<RabbitMqConsumerSettings>(settingsSection)
            .AddSingleton<IRabbitMqMessageProcessor, TMessageProcessor>()
            .AddSingleton<IRabbitMqConsumerInitializer, RabbitMqConsumerInitializer>()
            .AddHostedService<RabbitMqBackgroundService>();

        return services;
    }
}

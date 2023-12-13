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

        if (settings.Mode == RabbitMqConsumerMode.MultipleActiveConsumers)
            services
                .AddSingleton<IRabbitMqConsumerSemaphore, RabbitMqMultipleActiveConsumersSemaphore>();
        else
            services
                .AddSingleton<IRabbitMqConsumerSemaphore, RabbitMqSingleActiveConsumerSemaphore>();

        services
            .Configure<RabbitMqConsumerSettings>(settingsSection)
            .AddSingleton<IRabbitMqMessageProcessor, TMessageProcessor>()
            .AddSingleton<IRabbitMqConsumerInitializer, RabbitMqConsumerInitializer>()
            .AddHostedService<RabbitMqBackgroundService>();

        return services;
    }

    public static IServiceCollection AddRabbitMqConsumerLock<TLockProvider, TDomainObject, TDomainObjectBase>(
        this IServiceCollection services)
        where TLockProvider : class, IRabbitMqConsumerLockProviderService<TDomainObjectBase>
        where TDomainObjectBase : class
        where TDomainObject : TDomainObjectBase
    {
        services
            .AddScoped<IRabbitMqConsumerLockService, RabbitMqConsumerLockService<TDomainObject, TDomainObjectBase>>()
            .AddScoped<IRabbitMqConsumerLockProviderService<TDomainObjectBase>, TLockProvider>();

        return services;
    }
}

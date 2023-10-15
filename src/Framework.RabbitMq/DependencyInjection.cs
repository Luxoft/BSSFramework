using Framework.RabbitMq.Interfaces;
using Framework.RabbitMq.Services;
using Framework.RabbitMq.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.RabbitMq;

public static class DependencyInjection
{
    public static IServiceCollection AddRabbitMqClient(this IServiceCollection services, IConfiguration configuration) =>
        services
            .Configure<RabbitMqServerSettings>(configuration.GetSection("RabbitMQ:Server"))
            .AddSingleton<IRabbitMqClient, RabbitMqClient>();
}

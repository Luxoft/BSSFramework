using Framework.RabbitMq.Consumer.Interfaces;

namespace Framework.RabbitMq.Consumer.Services;

public record RabbitMqSqlSeverConnectionStringProvider(string ConnectionString) : IRabbitMqSqlSeverConnectionStringProvider
{
    public string GetConnectionString() => this.ConnectionString;
}

using Microsoft.Data.SqlClient;

namespace Framework.RabbitMq.Consumer.Interfaces;

public interface IRabbitMqConsumerLockService
{
    bool TryObtainLock(SqlConnection connection);

    void TryReleaseLock(SqlConnection connection);
}

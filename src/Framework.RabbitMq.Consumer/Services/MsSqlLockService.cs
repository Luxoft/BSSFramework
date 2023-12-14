using System.Data;

using Framework.RabbitMq.Consumer.Interfaces;
using Framework.RabbitMq.Consumer.Settings;

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Framework.RabbitMq.Consumer.Services;

internal record MsSqlLockService(IOptions<RabbitMqConsumerSettings> ConsumerOptions) : IRabbitMqConsumerLockService
{
    private readonly string _lockName = $"{ConsumerOptions.Value.Queue}_Consumer_Lock";

    public bool TryObtainLock(SqlConnection connection)
    {
        try
        {
            var cmd = new SqlCommand("sp_getapplock", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Resource", this._lockName));
            cmd.Parameters.Add(new SqlParameter("@LockMode", "Exclusive"));
            cmd.Parameters.Add(new SqlParameter("@LockOwner", "Session"));
            cmd.Parameters.Add(new SqlParameter("@LockTimeout", "0"));

            var returnValue = new SqlParameter { Direction = ParameterDirection.ReturnValue };
            cmd.Parameters.Add(returnValue);

            cmd.ExecuteNonQuery();

            return (int)returnValue.Value >= 0;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public void TryReleaseLock(SqlConnection connection)
    {
        try
        {
            var cmd = new SqlCommand("sp_releaseapplock", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@Resource", this._lockName));
            cmd.Parameters.Add(new SqlParameter("@LockOwner", "Session"));

            cmd.ExecuteNonQuery();
        }
        catch (Exception)
        {
            // ignored
        }
    }
}

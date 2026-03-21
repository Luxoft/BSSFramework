namespace Framework.Application.ConnectionStringSource;

public class ManualDefaultConnectionStringSource(string connectionString) : IDefaultConnectionStringSource
{
    public string ConnectionString { get; } = connectionString;
}

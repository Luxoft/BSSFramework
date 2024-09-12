namespace Framework.DomainDriven;

public class ManualDefaultConnectionStringSource(string connectionString) : IDefaultConnectionStringSource
{
    public string ConnectionString { get; } = connectionString;
}

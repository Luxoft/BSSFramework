using Microsoft.Extensions.Configuration;

namespace Framework.DomainDriven;

public class ConnectionStringSource(IConfiguration configuration, string name)
{
    public string ConnectionString => configuration.GetConnectionString(name)!;
}

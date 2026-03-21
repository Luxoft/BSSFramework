using Microsoft.Extensions.Configuration;

namespace Framework.Application.ConnectionStringSource;

public class ConnectionStringSource(IConfiguration configuration, string name)
{
    public string ConnectionString => configuration.GetConnectionString(name)!;
}

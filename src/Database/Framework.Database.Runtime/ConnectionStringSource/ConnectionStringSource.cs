using Microsoft.Extensions.Configuration;

namespace Framework.Database.ConnectionStringSource;

public class ConnectionStringSource(IConfiguration configuration, string name)
{
    public string ConnectionString =>
        configuration.GetConnectionString(name) ?? throw new InvalidOperationException($"{name} connection string is not configured.");
}

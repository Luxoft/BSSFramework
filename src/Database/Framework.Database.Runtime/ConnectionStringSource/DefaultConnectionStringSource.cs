using Microsoft.Extensions.Configuration;

namespace Framework.Database.ConnectionStringSource;

public class DefaultConnectionStringSource(IConfiguration configuration, DefaultConnectionStringSettings settings)
    : ConnectionStringSource(configuration, settings.Name), IDefaultConnectionStringSource;

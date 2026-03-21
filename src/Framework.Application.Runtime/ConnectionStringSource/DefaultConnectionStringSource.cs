using Microsoft.Extensions.Configuration;

namespace Framework.Application.ConnectionStringSource;

public class DefaultConnectionStringSource(IConfiguration configuration, DefaultConnectionStringSettings settings)
    : ConnectionStringSource(configuration, settings.Name), IDefaultConnectionStringSource;

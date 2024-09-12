using Microsoft.Extensions.Configuration;

namespace Framework.DomainDriven;

public class DefaultConnectionStringSource(IConfiguration configuration, DefaultConnectionStringSettings settings)
    : ConnectionStringSource(configuration, settings.Name), IDefaultConnectionStringSource;

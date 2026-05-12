using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.TestingProvider;

public interface IMsSqlServerSource
{
    Server Server { get; }
}

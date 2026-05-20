using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.TestingProvider;

public interface ISqlServerFactory
{
    Server Create();
}

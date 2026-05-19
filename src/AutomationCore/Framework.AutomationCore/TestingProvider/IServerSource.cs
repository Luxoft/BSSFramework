using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.TestingProvider;

public interface IServerSource
{
    Server Server { get; }
}

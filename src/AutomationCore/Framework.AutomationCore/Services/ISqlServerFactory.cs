using Microsoft.SqlServer.Management.Smo;

namespace Framework.AutomationCore.Services;

public interface ISqlServerFactory
{
    Server Create();
}


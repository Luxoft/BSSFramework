using System.Data;

using Framework.Cap.Abstractions;

namespace Automation.ServiceEnvironment.Services;

public class IntegrationTestCapTransactionManager : ICapTransactionManager
{
    public void Enlist(IDbTransaction dbTransaction)
    {
    }
}

using Anch.SecuritySystem.Testing;
using Anch.Testing;

using Framework.AutomationCore.ServiceEnvironment.Services;

namespace Framework.AutomationCore.Services;

public class BssCleanupTestEnvironmentHook(IntegrationTestTimeProvider timeProvider, RootImpersonateServiceState rootImpersonateServiceState) : ITestEnvironmentHook
{
    public ValueTask Process(CancellationToken _)
    {
        timeProvider.Reset();
        rootImpersonateServiceState.Reset();

        return ValueTask.CompletedTask;
    }
}

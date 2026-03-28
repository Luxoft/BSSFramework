using SecuritySystem.Credential;
using SecuritySystem.Testing;

namespace Framework.AutomationCore.ServiceEnvironment.ServiceEnvironment.Services;

public class BssTestingEvaluator<TService>(IServiceEvaluator<TService> serviceEvaluator) : ITestingEvaluator<TService>
    where TService : notnull
{
    public Task<TResult> EvaluateAsync<TResult>(TestingScopeMode mode, UserCredential? userCredential, Func<TService, Task<TResult>> evaluate) => serviceEvaluator.EvaluateAsync((DBSessionMode)mode, userCredential, evaluate);
}

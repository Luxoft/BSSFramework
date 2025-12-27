using Framework.DomainDriven;

using SecuritySystem.Testing;

namespace Automation.ServiceEnvironment.Services;

public class BssTestingEvaluator<TService>(IServiceEvaluator<TService> serviceEvaluator) : ITestingEvaluator<TService>
    where TService : notnull
{
    public Task<TResult> EvaluateAsync<TResult>(TestingScopeMode mode, Func<TService, Task<TResult>> evaluate) => serviceEvaluator.EvaluateAsync((DBSessionMode)mode, evaluate);
}

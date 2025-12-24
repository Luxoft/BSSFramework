using Framework.DomainDriven;

using SecuritySystem.Testing;

namespace Automation.ServiceEnvironment.Services;

public class BssTestingEvaluator<TService>(IServiceEvaluator<TService> serviceEvaluator) : ITestingEvaluator<TService>
    where TService : notnull
{
    public Task<TResult> EvaluateAsync<TResult>(Func<TService, Task<TResult>> evaluate) => serviceEvaluator.EvaluateAsync(DBSessionMode.Write, evaluate);
}

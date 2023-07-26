using Framework.DomainDriven;

namespace Framework.HangfireCore.JobServices;

public interface IServiceJobEvaluator
{
    public Task<TResult> ExecuteAsync<TService, TResult>(DBSessionMode sessionMode, Func<TService, Task<TResult>> evaluate);
}

public interface IServiceJobEvaluator<out TService>
{
    public Task<TResult> ExecuteAsync<TResult>(DBSessionMode sessionMode, Func<TService, Task<TResult>> evaluate);
}
